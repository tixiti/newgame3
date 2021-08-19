using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
/*[CustomEditor(typeof(StaticPlayerController))]
class DecalMeshHelperEditor : Editor {
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set Ray Length" ))
        {
            var controller = (StaticPlayerController) target;
            controller.SetRayLength(controller.rayRenderer.size.y/2f);
        }
    }
}*/
public class StaticPlayerController : MonoBehaviour
{
    public bool isSave;
    public SpriteRenderer rayRenderer;
    public Transform playerTransform;
    private GameObject _ball;
    private float _newLocalPosition;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.enabled = true;
    }

    public void SetRayLength(float rayLengthInput)
    {
        if (rayLengthInput==0)
        {
            rayRenderer.gameObject.SetActive(false);
        }
        else
        {
            rayRenderer.size = new Vector2(0.4f,2f* rayLengthInput);
            rayRenderer.gameObject.SetActive(true);
        }
        playerTransform.localPosition = Vector3.up * rayLengthInput;
        _newLocalPosition = rayLengthInput;
    }
    private static float ballScaleSpeed=0.02f,ballMoveSpeed=10;
    private CircleCollider2D _circleCollider2D;
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private Animator _animator;

    private void Update()
    {
        if (Mathf.Abs(playerTransform.localPosition.y-_newLocalPosition)>0.05f)
        {
            playerTransform.localPosition =
                Vector3.Lerp(playerTransform.localPosition, Vector3.up * _newLocalPosition, 10*Time.deltaTime);
        }

        if (!isSave) return;
        SetRayLength(rayRenderer.size.y/2.5f);
        isSave = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        gameObject.layer = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(3))
        {
            gameObject.layer = 2;
        }
        if (!other.gameObject.CompareTag("Ball")) return;
        // AudioController.instance.PlaySound(AudioController.instance.playerReceiveSound);
        _animator.SetBool(IsIdle,false);
        _animator.Play("Scale");
        GetComponent<CircleCollider2D>().enabled = false;
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
            _newLocalPosition = -_newLocalPosition;
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
