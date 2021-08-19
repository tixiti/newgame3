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
            UIController.instance.deadEffect.SetActive(false);
            GameController.instance.isPlaying = true;
            GameController.instance.timeStart = Time.time;
            ball.GetComponent<CircleCollider2D>().enabled = true;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
            if (MoveObject.currentPlayerSelected!=null&&Input.GetMouseButtonUp(0))
            {
                Destroy(MoveObject.currentPlayerSelected);
            }
        }
    }
}
