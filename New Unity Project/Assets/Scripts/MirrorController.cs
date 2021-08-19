using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnEnable()
    {
        var localScale = transform.localScale;
        if (localScale.x==1)
        {
            var parent = transform.parent;
            parent.GetChild(1).GetChild(0).localPosition = new Vector3(0, (localScale.y-1)*0.25f+0.5f, 1);
            parent.GetChild(1).GetChild(1).localPosition = new Vector3(0, -((localScale.y-1)*0.25f+0.5f), 1);
            parent.GetChild(1).eulerAngles = transform.eulerAngles;
            parent.GetChild(1).localPosition = transform.localPosition;

        }
        if (localScale.y==1)
        {
            var parent = transform.parent;
            parent.GetChild(1).GetChild(0).localPosition = new Vector3((localScale.x-1)*0.25f+0.5f,0, 1);
            parent.GetChild(1).GetChild(1).localPosition = new Vector3(-((localScale.x-1)*0.25f+0.5f),0, 1);
            parent.GetChild(1).eulerAngles = transform.eulerAngles;
            parent.GetChild(1).localPosition = transform.localPosition;
        }
        transform.parent.GetChild(1).gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameController.instance.isPlaying) return;
        var ballRb = other.GetComponent<Rigidbody2D>();
        if (!other.CompareTag("Ball")) return;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(ChangeDirection());
        IEnumerator ChangeDirection()
        {
            var velocity = ballRb.velocity;
            var x = transform.eulerAngles.z/180*Mathf.PI;
            var mirrorVertor = new Vector2(Mathf.Cos(x), Mathf.Sin(x));
            var angle = AngleBetweenVector2(ballRb.velocity, mirrorVertor)/180*Mathf.PI;
            var forceX = 10 * Mathf.Cos(angle + x);
            var forceY = 10 * Mathf.Sin(angle + x);
            ballRb.velocity = new Vector2(forceX, forceY);
            yield return new WaitForSeconds(0.1f);
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
        float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
        return Vector2.Angle(vec1, vec2) * sign;
    }
}
