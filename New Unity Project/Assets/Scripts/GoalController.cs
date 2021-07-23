using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")||!GameController.instance.isPlaying) return;
        GameController.instance.isPlaying = false;
        other.GetComponent<CircleCollider2D>().enabled = false;
        var ballRigidbody2D = other.GetComponent<Rigidbody2D>();
        if (other.GetComponent<BallController>().shieldCount>0)
        {
            StartCoroutine(PushNotification1());
            IEnumerator PushNotification1()
            {
                UIController.instance.PushNotification("FAIL!");
                yield return new WaitForSeconds(0.5f);
                GameController.instance.ResetLevel();
                ballRigidbody2D.velocity = Vector2.zero;
                GameController.instance.GenerateLevel(
                    GameDataController.instance.levelData[GameDataController.instance.currentLevel]);
            }

            return;
        }

        StartCoroutine(PushNotification2());
        IEnumerator PushNotification2()
        {
            UIController.instance.PushNotification("PASS!");
            yield return new WaitForSeconds(0.5f);
            GameController.instance.ResetLevel();
            ballRigidbody2D.velocity = Vector2.zero;
            GameDataController.instance.currentLevel += 1;
            GameDataController.instance.SaveData();
            GameController.instance.GenerateLevel(
                GameDataController.instance.levelData[GameDataController.instance.currentLevel]);
        }
        
    }
}
