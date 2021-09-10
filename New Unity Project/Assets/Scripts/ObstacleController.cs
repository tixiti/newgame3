using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameController.instance.isPlaying) return;
        if (!other.CompareTag("Ball")) return;
        AudioController.instance.PlaySound(AudioController.instance.obstacleDetectSound);
        other.GetComponent<CircleCollider2D>().enabled = false;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(PushNotification());
        IEnumerator PushNotification()
        {
            UIController.instance.deadEffect.SetActive(true);
            AudioController.instance.PlaySound(AudioController.instance.failSound);
            yield return new WaitForSeconds(0.5f);
            GameController.instance.ResetLevelSoft();
            GameController.instance.GenerateLevelSoft(GameDataController.instance.levelData[GameDataController.instance.currentLevel]);
        }
    }
}
