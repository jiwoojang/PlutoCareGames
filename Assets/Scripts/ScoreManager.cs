using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score = 0;
    private int leftScore = 0;
    private int rightScore = 0;

    [SerializeField]
    private Text scoreText;

    public void IncreaseLeftScore() {
        leftScore++;
        score++;
        Debug.Log("Incrementing Left Score");
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void IncreaseRightScore() {
        rightScore++;
        score++;
        Debug.Log("Incrementing Right Score");
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void ResetScore() {
        score = 0;
        Debug.Log("Reseting Score");
    }

    public int GetScore() {
        return score;
    }

    public int GetLeftScore() {
        return leftScore;
    }

    public int GetRightScore() {
        return rightScore;
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
