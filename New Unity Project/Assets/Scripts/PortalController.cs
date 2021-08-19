using System;
using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private GameObject portalToTransport;

    private void Start()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) yield break;
        GetComponent<CircleCollider2D>().enabled = false;
        var ball = other.gameObject;
        var oldForce = ball.GetComponent<Rigidbody2D>().velocity;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.transform.SetParent(portalToTransport.transform);
        portalToTransport.GetComponent<CircleCollider2D>().enabled = false;
        ball.transform.localPosition = Vector3.zero;
        ball.transform.SetParent(null);
        GetComponent<CircleCollider2D>().enabled = true;
        var forceX = oldForce.x * 10 / oldForce.magnitude;
        var forceY = oldForce.y * 10 / oldForce.magnitude;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(oldForce.x, oldForce.y);
        AudioController.instance.PlaySound(AudioController.instance.playerReceiveSound);
        yield return new WaitUntil(() =>
            Vector3.Distance(ball.transform.position, portalToTransport.transform.position) > 1f);
        portalToTransport.GetComponent<CircleCollider2D>().enabled = true;
    }
}