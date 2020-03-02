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

    public void CompleteLevel(int finalLeftHandScore, int finalRightHandScore) {
        levelLeftHandScore = finalLeftHandScore;
        levelRightHandScore = finalRightHandScore;
        levelFinalScore = finalLeftHandScore + finalRightHandScore;
        completionState = "completed";
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

    public List<LevelData> levelData = new List<LevelData>();

    public GameData(string id) {
        uid = id;
    }

    public void FinalizeSessionData(int score) {
        finalScore = score;
        levelCount = levelData.Count;

        foreach(LevelData data in levelData) {
            if (data.completionState == "completed") {
                successes++;
            }
            else {
                failures++;
            }
        }
    }

    public void AddLevelData(int levelNum, int scoreToComplete, int levelTime) {
        levelData.Add(new LevelData(levelNum, scoreToComplete, levelTime));
    }

    public string ConvertToJSON() {
        return JsonUtility.ToJson(this);
    }
}
