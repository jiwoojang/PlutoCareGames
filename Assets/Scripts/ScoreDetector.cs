using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    public bool shouldDetectLeft;
    public GameObject debug;

    // Basic collision based score increment
    private void OnTriggerEnter(Collider other) {

        // If we cannot score, then no need to do anything else
        if (GameManager.instance.state != GameManager.ScoreState.CanScore) {
            return;
        }

        if (shouldDetectLeft) {
            if (other.gameObject.tag == "LeftHandCollider") {
                ScoreManager.instance.IncreaseLeftScore();
            }
        } else {
            if (other.gameObject.tag == "RightHandCollider") {
                ScoreManager.instance.IncreaseRightScore();
            }
        }
    }
}
