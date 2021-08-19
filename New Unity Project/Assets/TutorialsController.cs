using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsController : MonoBehaviour
{
    private GameObject Player;
    private Vector3 rightPosition = new Vector3(0, 3.41f, 0);
    private void Start()
    {
        Player = GameObject.Find("Player Prefab (1)");
    }

    private void Update()
    {
        if (Vector3.Distance(Player.transform.position,rightPosition)<0.5f&&!transform.GetChild(2).gameObject.activeInHierarchy)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        
    }
}
