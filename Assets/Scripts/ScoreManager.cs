using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleton Access
    public static ScoreManager instance;

    private int leftScore = 0;
    private int rightScore = 0;

    private int totalScore = 0;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text finalScoreText;

    public void IncreaseLeftScore() {
        leftScore++;
        Debug.Log("Incrementing Left Score");
        scoreText.text = "SCORE: " + (leftScore + rightScore).ToString();
    }

    public void IncreaseRightScore() {
        rightScore++;
        Debug.Log("Incrementing Right Score");
        scoreText.text = "SCORE: " + (leftScore + rightScore).ToString();
    }

    public void SetTotalScore() {
        totalScore += leftScore + rightScore;
    }

    public void ResetScore() {
        leftScore = 0;
        rightScore = 0;
        Debug.Log("Reseting Score");
    }

    public int GetLeftScore() {
        return leftScore;
    }

    public int GetRightScore() {
        return rightScore;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public void FinalizeScore() {
        finalScoreText.text = "FINAL SCORE\n" + totalScore.ToString();
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("More than once instance of the ScoreManager Singleton. Deleting the old instance.");
            DestroyImmediate(instance);

            instance = this;
        }
    }
}
