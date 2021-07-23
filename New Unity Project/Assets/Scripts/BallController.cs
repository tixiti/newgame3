using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject[] shield;
    public int shieldCount;
    private void Start()
    {
        ResetShield();
    }

    public void ResetShield()
    {
        shieldCount = 3;
        for (var i = 0; i < shield.Length; i++)
        {
            shield[i].SetActive(true);
        }
    }
    public void BreakShield()
    {
        shield[shieldCount-1].SetActive(false);
        shieldCount -= 1;
    }
}
