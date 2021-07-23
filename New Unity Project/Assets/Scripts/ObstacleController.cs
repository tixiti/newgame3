using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameController.instance.isPlaying) return;
        if (!other.CompareTag("Ball")) return;
        other.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(PushNotification());
        IEnumerator PushNotification()
        {
            UIController.instance.PushNotification("FAIL!");
            yield return new WaitForSeconds(0.5f);
            GameController.instance.ResetLevelSoft();
        }
    }
}
