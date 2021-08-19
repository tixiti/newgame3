using System;
using System.Collections;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField] private GameObject collectEffect;
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject flashEffect;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();    }

    public void Reset()
    {
        collectEffect.SetActive(false);
        _circleCollider2D.enabled = true;
        _spriteRenderer.enabled = true;      
        flashEffect.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        AudioController.instance.PlaySound(AudioController.instance.bubbleSound);
        other.GetComponent<BallController>().BreakShield();
        _circleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
        flashEffect.SetActive(false);
        collectEffect.SetActive(true);
    }
}
