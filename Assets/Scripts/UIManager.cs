using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton Access 
    public static UIManager instance;

    public GameObject LevelUI;
    public GameObject IntroUI;
    public GameObject EndGameUI;
    public GameObject TransitionUI;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("More than once instance of the UIManager Singleton. Deleting the old instance.");
            DestroyImmediate(instance);

            instance = this;
        }
    }

    // For starting up the right UI
    public void InitializeIntroUI() {
        if (EndGameUI != null )
            EndGameUI.SetActive(false);

        if (LevelUI != null)
            LevelUI.SetActive(false);

        if (IntroUI != null)
            IntroUI.SetActive(true);

        if (TransitionUI != null)
            TransitionUI.SetActive(false);
    }
    public void InitializeLevelUI() {
        if (EndGameUI != null)
            EndGameUI.SetActive(false);

        if (LevelUI != null)
            LevelUI.SetActive(true);

        if (IntroUI != null)
            IntroUI.SetActive(false);

        if (TransitionUI != null)
            TransitionUI.SetActive(false);
    }
    public void InitializeEndGameUI() {
        if (EndGameUI != null)
            EndGameUI.SetActive(true);

        if (LevelUI != null)
            LevelUI.SetActive(false);

        if (IntroUI != null)
            IntroUI.SetActive(false);

        if (TransitionUI != null)
            TransitionUI.SetActive(false);
    }

    public void InitializeTransitionUI() {
        if (EndGameUI != null)
            EndGameUI.SetActive(false);

        if (LevelUI != null)
            LevelUI.SetActive(false);

        if (IntroUI != null)
            IntroUI.SetActive(false);

        if (TransitionUI != null)
            TransitionUI.SetActive(true);
    }
}
