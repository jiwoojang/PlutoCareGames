using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    public enum CollisionSource
    {
        None,
        LeftHand,
        RightHand,
    }

    public GameObject debug;
    public CollisionSource collisionSource;

    // Basic collision based score increment
    private void OnTriggerEnter(Collider other) {

        // If we cannot score, then no need to do anything else
        if (GameManager.instance.scoreState != GameManager.ScoreState.CanScore) {
            return;
        }

        if (collisionSource == CollisionSource.LeftHand) {
            if (other.gameObject.tag == "LeftHandCollider" || other.gameObject.tag == "AllHandsCollider") {
                ScoreManager.instance.IncreaseLeftScore();
                return;
            } else if (other.gameObject.tag == "IntroLeftCollider" || other.gameObject.tag == "IntroAllCollider") {
                GameManager.instance.AdvanceIntro();
            }
        }
        
        if (collisionSource == CollisionSource.RightHand){
            if (other.gameObject.tag == "RightHandCollider") {
                ScoreManager.instance.IncreaseRightScore();
                return;
            } else if (other.gameObject.tag == "IntroRightCollider") {
                GameManager.instance.AdvanceIntro();
            }
        }
    }
}
