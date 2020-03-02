using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeyInputCollector : MonoBehaviour
{
    [SerializeField]
    private int maxInputCharacters = 4;

    [SerializeField]
    private Text inputDisplayText;

    private string currentInput;

    private void Awake() {
        currentInput = "";
        UpdateInput();
    }

    private void UpdateInput() {

        string currentInputDisplay = currentInput;

        for (int i = currentInputDisplay.Length; i < maxInputCharacters; i++) {
            currentInputDisplay += "-";
        }

        inputDisplayText.text = currentInputDisplay;
    }

    public void Input(char inputChar) {
        if (currentInput.Length < maxInputCharacters) {
            // Not the best way to append to strings I know, but using Stringbuilder seems like overkill here...
            currentInput += inputChar;

            Debug.Log("Inputing character " + inputChar.ToString());
            UpdateInput();
        }
    }

    public void DeleteLastChar() {
        if(currentInput.Length > 0) {
            Debug.Log("Deleting Character");
            currentInput = currentInput.Remove(currentInput.Length - 1);
            UpdateInput();
        }
    }

    public void Commit() {
        // Don't allow commits for UUIDs that are too short
        if (currentInput.Length < maxInputCharacters) {
            Debug.Log("Attempting to commit a UUID that is too short");
            return;
        }

        UUIDManager.instance.SetUUIDString(currentInput);
        Debug.Log("Committing UUID: " + currentInput);

        // Load next scene (AKA the game!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
