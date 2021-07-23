using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonController : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float speed;
    private void OnMouseUpAsButton()
    {
        if (!GameController.instance.isPlaying)
        {
            GameController.instance.isPlaying = true;
            GameController.instance.timeStart = Time.time;
            ball.GetComponent<CircleCollider2D>().enabled = true;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        }
    }
}
