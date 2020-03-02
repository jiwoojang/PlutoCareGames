using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyInput : MonoBehaviour
{
    public enum KeyType
    {
        Number,
        Delete,
        Commit
    }

    [SerializeField]
    private KeyType keyType;

    [SerializeField]
    private char inputChar;

    [SerializeField]
    private Text displayText;

    [SerializeField]
    private float scaleAmount;

    [SerializeField]
    private KeyInputCollector inputCollector;

    private Vector3 originalScale = Vector3.one;

    // Start is called before the first frame update
    void Start()
    {
        if (keyType != KeyType.Number) {
            inputChar = '\0';
        }
        else {
            displayText.text = inputChar.ToString();
        }

        if (inputCollector == null) {
            Debug.LogError("Cannot find collector for input, destroying this key input GameObject");
            Destroy(gameObject);
        }

        originalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other) {
        transform.localScale = new Vector3(originalScale.x * scaleAmount, originalScale.y * scaleAmount, originalScale.z * scaleAmount);

        if (inputCollector != null) {
            switch (keyType) {
                case KeyType.Number: {
                    inputCollector.Input(inputChar);
                    break;
                }
                case KeyType.Delete: {
                    inputCollector.DeleteLastChar();
                    break;
                }
                case KeyType.Commit: {
                    inputCollector.Commit();
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        transform.localScale = originalScale;
    }
}
