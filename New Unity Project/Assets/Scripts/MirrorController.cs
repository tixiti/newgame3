using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
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
