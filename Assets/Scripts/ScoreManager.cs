using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score = 0;

    [SerializeField]
    private Text scoreText;

    public void IncreaseScore() {
        score++;
        Debug.Log("Incrementing Score");
        scoreText.text = "SCORE: " + score.ToString();
    }

    public void WriteGameObjectName(string name) {
        scoreText.text = name;
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
