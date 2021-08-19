using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject _ball;

    [SerializeField] private float ballScaleSpeed,ballMoveSpeed;
    private CircleCollider2D _circleCollider2D;
    [NonSerialized]public Vector3 rightPosition;
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        // AudioController.instance.PlaySound(AudioController.instance.playerReceiveSound);
        _animator.SetBool(IsIdle,false);
        _animator.Play("Scale");
        _circleCollider2D.enabled = false;
        _ball = other.gameObject;
        var ballRigidbody2D = _ball.GetComponent<Rigidbody2D>();
        StartCoroutine(ShootBall());
        IEnumerator ShootBall()
        {
            ballRigidbody2D.velocity = Vector2.zero;
            _ball.transform.SetParent(this.transform);
            _ball.transform.localPosition = Vector3.zero;
            _ball.transform.localScale = Vector3.one / 2;
            yield return new WaitForSeconds(0.5f);
            _ball.transform.SetParent(null);
            var zRotation = (float)transform.eulerAngles.z/180.0f*Mathf.PI;
            var forceX = ballMoveSpeed*Mathf.Cos(zRotation);
            var forceY = ballMoveSpeed*Mathf.Sin(zRotation);
            ballRigidbody2D.velocity = new Vector2(forceX, forceY);
            AudioController.instance.PlaySound(AudioController.instance.shootSound);
            while (_ball.transform.localScale.x < 1)
            {
                yield return new WaitForEndOfFrame();
                _ball.transform.localScale += Vector3.one * ballScaleSpeed;
            }
        }
        StartCoroutine(CheckConditionToEnableCollider());
        IEnumerator CheckConditionToEnableCollider()
        {
            yield return new WaitUntil(() => Vector3.Distance(_ball.transform.position, transform.position) > 1f);
            _circleCollider2D.enabled = true;
            _animator.SetBool(IsIdle,true);
        }
    }
}
