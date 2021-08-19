using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
    public static HintController instance;
    public int currentHint;
    const float SpeedMove = 10f;
    private void Awake()
    {
        instance = this;
        currentHint = 0;
    }

    public void Reset()
    {
        currentHint = 0;
    }

    public void Next()
    {
        var gdc = GameDataController.instance;
        var gc = GameController.instance;
        if (currentHint<gdc.levelData[gdc.currentLevel].playerData.Count-1)
        {
            StartCoroutine(MoveToRightPosition());
            IEnumerator MoveToRightPosition()
            {
                var clone = gc.playerPrefabs[currentHint+1];
                clone.GetComponent<CircleCollider2D>().enabled = false;
                while (Vector3.Distance(clone.transform.position,clone.GetComponent<PlayerController>().rightPosition)>0.1f)
                {
                    clone.transform.position = Vector3.MoveTowards(clone.transform.position,
                        clone.GetComponent<PlayerController>().rightPosition, SpeedMove * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                clone.transform.position = clone.GetComponent<PlayerController>().rightPosition;
                clone.GetComponent<CircleCollider2D>().enabled = true;
            }
            currentHint++;
            return;
        }

        if (currentHint<gdc.levelData[gdc.currentLevel].playerData.Count-1+gdc.levelData[gdc.currentLevel].rotatingPlayerData.Count)
        {
            StartCoroutine(MoveToRightPosition());
            IEnumerator MoveToRightPosition()
            {
                var clone = gc.rotatingPlayerPrefabs[currentHint-gdc.levelData[gdc.currentLevel].playerData.Count+1];
                clone.GetComponent<CircleCollider2D>().enabled = false;
                while (Vector3.Distance(clone.transform.position,clone.GetComponent<RotatingPlayerController>().rightPosition)>0.1f)
                {
                    clone.transform.position = Vector3.MoveTowards(clone.transform.position,
                        clone.GetComponent<RotatingPlayerController>().rightPosition, SpeedMove * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                clone.transform.position = clone.GetComponent<RotatingPlayerController>().rightPosition;
                clone.GetComponent<CircleCollider2D>().enabled = true;
            }
            currentHint++;
        }
    }
}
