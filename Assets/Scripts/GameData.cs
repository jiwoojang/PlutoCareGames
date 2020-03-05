using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public int levelNumber;
    public int levelScoreToComplete;
    public int levelTimeToComplete;
    public int levelLeftHandScore;
    public int levelRightHandScore;
    public float levelTimeToReachThresholdScore;
    public int levelFinalScore;
    public string completionState = "not_complete";

    public LevelData(int levelNum, int scoreToComplete, int levelTime) {
        levelNumber = levelNum;
        levelScoreToComplete = scoreToComplete;
        levelTimeToComplete = levelTime;
        levelLeftHandScore = 0;
        levelRightHandScore = 0;
        levelFinalScore = 0;
    }

    public void CompleteLevel(int finalLeftHandScore, int finalRightHandScore, float timeToBeat) {
        levelLeftHandScore = finalLeftHandScore;
        levelRightHandScore = finalRightHandScore;
        levelFinalScore = finalLeftHandScore + finalRightHandScore;
        levelTimeToReachThresholdScore = timeToBeat;
        completionState = "completed";
    }
}

[Serializable]
public class LevelDataWrapper
{
    public List<LevelData> data = new List<LevelData>();

    public LevelDataWrapper(List<LevelData> newData) {
        data = newData;
    }
}

[Serializable]
public class GameData
{
    public string uid;
    public string game_title = "Boxing_Game";
    public string task = "Bilateral_Coordination";

    public int finalScore = 0;
    public int successes = 0;
    public int failures = 0;

    public int levelCount = 0;

    public string levelData;

    private List<LevelData> levelDataContainer = new List<LevelData>();

    public GameData(string id) {
        uid = id;
        levelData = "";
    }

    public void FinalizeSessionData (int score) {
        finalScore = score;
        levelCount = levelDataContainer.Count;

        foreach(LevelData data in levelDataContainer) {
            if (data.completionState == "completed") {
                successes++;
            }
            else {
                failures++;
            }
        }
    }

    public void CompleteLevel(int levelNumber, int finalLeftScore, int finalRightScore, float timeToBeat) {
        levelDataContainer[levelNumber].CompleteLevel(finalLeftScore, finalRightScore, timeToBeat);
    }

    public void AddLevelData(int levelNum, int scoreToComplete, int levelTime) {
        levelDataContainer.Add(new LevelData(levelNum, scoreToComplete, levelTime));
    }

    public string ConvertToJSON() {
        string levelDataString = JsonUtility.ToJson(new LevelDataWrapper(levelDataContainer));
        string base64LevelData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(levelDataString));

        levelData = base64LevelData;

        return JsonUtility.ToJson(this);
    }
}
