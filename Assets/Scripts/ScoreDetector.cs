using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    public bool shouldDetectLeft;
    public GameObject debug;

    private void OnTriggerEnter(Collider other) {
        if (shouldDetectLeft) {
            if (other.gameObject.tag == "LeftHandCollider") {
                ScoreManager.instance.IncreaseScore();
            }
        } else {
            if (other.gameObject.tag == "RightHandCollider") {
                ScoreManager.instance.IncreaseScore();
            }
        }
    }
}
