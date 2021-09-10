using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var gdc = GameDataController.instance;
        var gc = GameController.instance;
        if (!other.CompareTag("Ball")||!gc.isPlaying) return;
      
        gc.isPlaying = false;
        other.GetComponent<CircleCollider2D>().enabled = false;
        var ballRenderer = other.GetComponent<SpriteRenderer>();
        var ballRigidbody2D = other.GetComponent<Rigidbody2D>();
        ballRigidbody2D.velocity = Vector2.zero;
        if (other.GetComponent<BallController>().shieldCount>0)
        {
            StartCoroutine(PushNotification1());
            IEnumerator PushNotification1()
            {
                AudioController.instance.PlaySound(AudioController.instance.failSound);
                UIController.instance.deadEffect.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                gc.ResetLevelSoft();
                gc.GenerateLevelSoft(
                    gdc.levelData[gdc.currentLevel]);
            }

            return;
        }

        StartCoroutine(PushNotification2());
        IEnumerator PushNotification2()
        {
            for (int i = 1; i < gdc.levelData[gdc.currentLevel].playerData.Count; i++)
            {
                gdc.levelData[gdc.currentLevel].playerData[i].position = new Vector(gc.playerPrefabs[i].transform.position);
            }
            for (int i = 0; i < gdc.levelData[gdc.currentLevel].rotatingPlayerData.Count; i++)
            {
                gdc.levelData[gdc.currentLevel].rotatingPlayerData[i].position = new Vector(gc.rotatingPlayerPrefabs[i].transform.position);
            }
            AudioController.instance.PlaySound(AudioController.instance.ballBreakSound);
            UIController.instance.winEffect.Play();
            ballRenderer.enabled = false;
            if (gdc.currentLevel==55)
            {
                gdc.currentLevel += 1;
                if (gdc.maxLevel<gdc.currentLevel)
                {
                    gdc.maxLevel = gdc.currentLevel;
                }
                gdc.SaveData();
                UIController.instance.PushNotification("Congratulations! My Dinh's waiting for you!",5);
                var clone = Instantiate(UIController.instance.doneMapEffect);
                yield return new WaitForSeconds(5);
                Destroy(clone);
                UIController.instance.BackToMap();
            }
            else
            {
                gdc.currentLevel += 1;
                if (gdc.maxLevel<gdc.currentLevel)
                {
                    gdc.maxLevel = gdc.currentLevel;
                }
                gdc.SaveData();
                UIController.instance.PushNotification("PASS!");
            }
            HintController.instance.Reset();
            yield return new WaitForSeconds(0.5f);
            gdc.SaveData();
            gc.RenderLevel(
                gdc.currentLevel);
        }
        
    }
}
