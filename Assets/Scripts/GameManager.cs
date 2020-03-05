using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    // Singleton access
    public static GameManager instance;

    // Stores the configuration for a level
    // One level contains:
    // One or many punching bags at unique configurations
    // A total time permitted for the level 
    // A score threshold to beat the level
    [System.Serializable]
    public class LevelConfiguration
    {
        public int levelTime;
        public int thresholdScore;
        public GameObject bagConfiguration;
    }

    [SerializeField]
    private Transform headTransform;

    [SerializeField]
    private List<LevelConfiguration> levels = new List<LevelConfiguration>();

    [SerializeField]
    private float angleThresholdDegrees = 30.0f;

    [SerializeField]
    private int transitionWaitTime = 3;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private SkinnedMeshRenderer leftHandRenderer;
    private Color initialLeftHandColor;

    [SerializeField]
    private SkinnedMeshRenderer rightHandRenderer;
    private Color initialRightHandColor;

    [SerializeField]
    private Color multiHandInteractionColor;

    [SerializeField]
    private List<GameObject> introButtons = new List<GameObject>();

    private int introButtonIndex = 0;
    private int levelIndex = 0;
    private float angleThresholdDot;
    private LevelConfiguration currentConfig;
    private float levelStartTime = 0.0f;
    private float levelScoreThresholdTime = 0.0f;

    public enum ScoreState
    {
        None,
        CanScore, 
        CannotScore
    }

    public enum GameState
    {
        Intro,
        Levels,
        Endgame
    }

    public ScoreState scoreState = ScoreState.None;
    public GameState gameState = GameState.Intro;

    private GameData sessionData;

    private void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("More than once instance of the GameManager Singleton. Deleting the old instance.");
            DestroyImmediate(instance);

            instance = this;
        }

        if (headTransform == null) {
            Debug.LogError("Missing Head Transform! Game cannot continue correctly");
        }

        // This number converts the look threshold to a dot product-comparable value
        // Save us from computing it every frame
        angleThresholdDot = Mathf.Cos(Mathf.Deg2Rad * angleThresholdDegrees);
        scoreState = ScoreState.CannotScore;
        levelIndex = 0;
        introButtonIndex = 0;
        gameState = GameState.Intro;
        levelStartTime = 0.0f;
        levelScoreThresholdTime = 0.0f;

        // Set up the intro buttons correctly
        foreach (GameObject gameObject in introButtons) {
            gameObject.SetActive(false);
        }

        initialLeftHandColor = leftHandRenderer.material.color;
        initialRightHandColor = rightHandRenderer.material.color;


        // Do this here incase we came from a restart
        RevertHandColors();

        // Initialize API data for this session
        sessionData = new GameData(UUIDManager.instance.GetUUIDString());

        // Start the intro action
        StartIntro();
    }

    private void StartIntro() {
        // Put up the right UI
        UIManager.instance.InitializeIntroUI();

        if (introButtons.Count == 0)
            return;

        // Put up the first button
        introButtons[introButtonIndex].SetActive(true);
    }

    public void AdvanceIntro() {
        Debug.Log("Advancing Intro");

        if (introButtons.Count == 0)
            return;

        // Disable the current button
        introButtons[introButtonIndex].SetActive(false);

        introButtonIndex++;

        if (introButtonIndex == introButtons.Count) {
            StartCoroutine(TransitionToLevels());
            return;
        }

        introButtons[introButtonIndex].SetActive(true);
    }

    private IEnumerator TransitionToLevels() {
        UIManager.instance.InitializeTransitionUI();

        yield return new WaitForSeconds(transitionWaitTime);

        gameState = GameState.Levels;
        UIManager.instance.InitializeLevelUI();
        StartNextLevel();
    }

    public void StartNextLevel() {
        if (levelIndex < levels.Count) {
            Debug.Log("Starting Level " + levelIndex);
            levelText.text = "LEVEL: " + (levelIndex + 1).ToString();
            StartCoroutine(RunLevel(levels[levelIndex]));
            levelIndex++;
        }
        else {
            Debug.Log("Finished All Levels");
            EndGame();
        }
    }
    
    public void EndGame() {
        gameState = GameState.Endgame;
        scoreState = ScoreState.CanScore;

        // Return everything to normal transparency
        UpdateTransparency(currentConfig.bagConfiguration, 1.0f);

        // Clean up all punching bags, hands, and buttons
        currentConfig.bagConfiguration.SetActive(false);

        // Set hands to universal interaction colors
        SetMultiHandColors();

        // Set final score
        sessionData.FinalizeSessionData(ScoreManager.instance.GetTotalScore());
        Debug.Log("Ending Game");
        StartCoroutine(SendAPIData());
    }

    private void ShowEndGameState() {
        UIManager.instance.InitializeEndGameUI();
        ScoreManager.instance.FinalizeScore();
    }

    IEnumerator SendAPIData() {
        Debug.Log("Sending API Data:");
        Debug.Log(sessionData.ConvertToJSON());

        // Make our session data into a JSON Object
        using (UnityWebRequest www = UnityWebRequest.Put("https://prd-sql01.ddns.net/game/session_entry", sessionData.ConvertToJSON())) {

            // Set appropriate headers as specified by PlutoCare
            www.SetRequestHeader("Authorization", "Basic Z2FtZV9kZXY6cGx1dG9jYXJlZGV2dGVhbQ==");
            www.SetRequestHeader("Content-Type", "application/json");

            // Send it and wait for results!
            yield return www.SendWebRequest();
            Debug.Log("Response Code: " + www.responseCode);

            if (www.isNetworkError || www.isHttpError) {
                Debug.LogError("API Upload Error");
                Debug.LogError(www.error);
            } else {
                Debug.Log("Successfully sent API Message!");
                Debug.Log("Response Code: " + www.responseCode);
            }

            // Make API failure non-blocking
            ShowEndGameState();
        }

        //yield return null;
    }

    public void CheckScoreThreshold(int newScore) {
        if (currentConfig != null && levelScoreThresholdTime == 0.0f && newScore >= currentConfig.thresholdScore) {
            levelScoreThresholdTime = Time.unscaledTime;
        }
    }

    private IEnumerator RunLevel(LevelConfiguration config) {
        // Deactivate bags from previous config
        if (currentConfig != null) {
            if (currentConfig.bagConfiguration != null) {
                currentConfig.bagConfiguration.SetActive(false);
            }
        }

        // Setup new config
        currentConfig = config;

        // Add this config's data to the session data for the API
        sessionData.AddLevelData(levelIndex + 1, currentConfig.thresholdScore, currentConfig.levelTime);

        if (currentConfig.bagConfiguration != null) {
            currentConfig.bagConfiguration.SetActive(true);
        }

        levelStartTime = Time.unscaledTime;

        // Special case for middle level
        if (levelIndex == 2) {
            // Update colors of hands
            SetMultiHandColors();
        }
        else {
            // Update colors of hands
            RevertHandColors();
        }

        int counter = currentConfig.levelTime;

        // Initialize timer for the level
        Debug.Log("Time: " + counter);
        timerText.text = "TIME: " + counter;

        while (counter > 0) {
            yield return new WaitForSeconds(1);
            counter--;
            
            // Count down timer
            Debug.Log("Time: " + counter);
            timerText.text = "TIME: " + counter;
        }

        // If we are here the timer is done.
        // See if the score threshold was met!
        int leftHandScore = ScoreManager.instance.GetLeftScore();
        int rightHandScore = ScoreManager.instance.GetRightScore();

        if (leftHandScore >= currentConfig.thresholdScore || rightHandScore >= currentConfig.thresholdScore) {
            sessionData.CompleteLevel(levelIndex - 1, leftHandScore, rightHandScore, levelScoreThresholdTime - levelStartTime);
            ScoreManager.instance.SetTotalScore();
            ScoreManager.instance.ResetScore();

            levelScoreThresholdTime = 0.0f;
            levelStartTime = 0.0f;

            StartNextLevel();
        }
        else {
            EndGame();
        }
    }

    void SetMultiHandColors() {
        if (leftHandRenderer != null) {
            leftHandRenderer.material.color = multiHandInteractionColor;
        }

        if (rightHandRenderer != null) {
            rightHandRenderer.material.color = multiHandInteractionColor;
        }
    }

    void RevertHandColors() {
        if (leftHandRenderer != null) {
            leftHandRenderer.material.color = initialLeftHandColor;
        }

        if (rightHandRenderer != null) {
            rightHandRenderer.material.color = initialRightHandColor;
        }
    }

    void UpdateTransparency(GameObject targetGroup, float newAlpha) {
        // Update colors of hands
        if (leftHandRenderer != null) {
            Color currentColor = leftHandRenderer.material.color;
            currentColor.a = newAlpha;

            leftHandRenderer.material.color = currentColor;
        }

        if (rightHandRenderer != null) {
            Color currentColor = rightHandRenderer.material.color;
            currentColor.a = newAlpha;

            rightHandRenderer.material.color = currentColor;
        }

        // Update all transparencies for punching bags
        Component[] renderers = targetGroup.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in renderers) {
            Color currentColor = renderer.material.color;
            currentColor.a = newAlpha;

            renderer.material.color = currentColor;
        }
    }

    void Update() {

        if (gameState == GameState.Intro || gameState == GameState.Levels) {
            // Get the dot product between the head's forward direction and the world forward direction
            float lookDeviation = Vector3.Dot(Vector3.forward, headTransform.forward);

            // Only process a threshold violation if changing states
            if (Mathf.Abs(lookDeviation) > angleThresholdDot) {
                if (scoreState == ScoreState.CannotScore) {
                    scoreState = ScoreState.CanScore;

                    // Update visuals here
                    if (gameState == GameState.Levels)
                        UpdateTransparency(currentConfig.bagConfiguration, 1.0f);
                    else
                        UpdateTransparency(introButtons[introButtonIndex], 1.0f);
                }
            } else {
                if (scoreState == ScoreState.CanScore) {
                    scoreState = ScoreState.CannotScore;

                    // Update visuals again
                    if (gameState == GameState.Levels)
                        UpdateTransparency(currentConfig.bagConfiguration, 0.2f);
                    else
                        UpdateTransparency(introButtons[introButtonIndex], 0.2f);
                }
            }
        }
    }
}
