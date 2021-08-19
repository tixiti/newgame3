using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlayerController : MonoBehaviour
{
    private GameObject _ball;

    [SerializeField] private float ballScaleSpeed,ballMoveSpeed,firstSpeed;

    private Transform _transform;
    [NonSerialized]public Vector3 rightPosition;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.enabled = true;
        _transform = transform;
    }

    private float _speedLerp;
    private void Update()
    {
        if (_transform.GetChild(2).gameObject.activeInHierarchy) return;
        _speedLerp = firstSpeed* Time.deltaTime;
        if (_oldChildRotation>0)
        {
            _oldChildRotation -= _speedLerp;
                if (_oldChildRotation<0)
                {
                    _transform.eulerAngles += new Vector3(0, 0, _speedLerp+_oldChildRotation);
                    _transform.GetChild(2).gameObject.SetActive(true);
                    _transform.GetChild(2).localEulerAngles = -_transform.GetChild(2).localEulerAngles;
                    return;
                }
                _transform.eulerAngles += new Vector3(0, 0, _speedLerp);
                
        }
        else
        {

                _oldChildRotation += _speedLerp;
                if (_oldChildRotation>0)
                {
                    _transform.eulerAngles -= new Vector3(0, 0, _speedLerp-_oldChildRotation);
                    _transform.GetChild(2).gameObject.SetActive(true);
                    _transform.GetChild(2).localEulerAngles = -_transform.GetChild(2).localEulerAngles;
                    return;
                }
                _transform.eulerAngles -= new Vector3(0, 0, _speedLerp);
        }

    }

    private void OnDisable()
    {
        if (!_transform.GetChild(2).gameObject.activeInHierarchy)
        {
            _transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle>180)
        {
            return angle - 360;
        }

        return angle;
    }
    private float _oldChildRotation;
    private CircleCollider2D _circleCollider2D;
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private Animator _animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        // AudioController.instance.PlaySound(AudioController.instance.playerReceiveSound);
        _oldChildRotation = WrapAngle(_transform.GetChild(2).localEulerAngles.z);
        _animator.SetBool(IsIdle,false);
        _animator.Play("Scale");
        GetComponent<CircleCollider2D>().enabled = false;
        _ball = other.gameObject;
        var ballRigidbody2D = _ball.GetComponent<Rigidbody2D>();
        StartCoroutine(ShootBall());
        IEnumerator ShootBall()
        {
            ballRigidbody2D.velocity = Vector2.zero;
            _ball.transform.SetParent(this._transform);
            _ball.transform.localPosition = Vector3.zero;
            _ball.transform.localScale = Vector3.one / 2;
            yield return new WaitForSeconds(0.5f);
            _ball.transform.SetParent(null);
            _transform.GetChild(2).gameObject.SetActive(false);
            var zRotation = (float)_transform.eulerAngles.z/180.0f*Mathf.PI;
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
