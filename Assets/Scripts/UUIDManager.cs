using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UUIDManager : MonoBehaviour
{
    public static UUIDManager instance;

    private string uuidString;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("More than once instance of the UUIDManager Singleton. Deleting the old instance.");
            DestroyImmediate(instance);

            instance = this;
        }

        // Make sure this UUID works through out all subsequent sessions
        DontDestroyOnLoad(gameObject);

        uuidString = "";
    }

    public void SetUUIDString(string newID) {
        uuidString = newID;
    }

    public string GetUUIDString() {
        return uuidString;
    }
}
