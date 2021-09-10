using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject moveObject;
    public static GameObject currentMoveArea;
    private Vector3 _oldPosition;
    private Camera _camera;
    private Vector3 _oldMousePosition;
    private void Start()
    {
        _camera = Camera.main;
    }

    public struct StaticObjectTouches
    {
        public Touch touch;
        public GameObject currentTouchedObject;
        public SpriteRenderer x;
        public SpriteRenderer y;
    }
    private List<StaticObjectTouches> _staticObjectTouchesList = new List<StaticObjectTouches>();
    private StaticObjectTouches _moveTouch;
    private bool _isDragging;
    public static bool isCanSelectToMove = true;
    private void Update()
    {
        if (GameController.instance.isPlaying) return;
        if (Input.touchCount>0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                switch (Input.touches[i].phase)
                {
                    case TouchPhase.Began:
                        if (MenuClickController.animator.GetCurrentAnimatorStateInfo(0).IsName("MenuBtnOpen"))
                            MenuClickController.animator.Play("MenuBtnClose");
                        if (!hit)
                        {
                            if (!isCanSelectToMove&&!_isDragging)
                            {
                                if (currentMoveArea!=null)
                                {
                                    var animator = MoveObject.currentMoveArea.GetComponent<Animator>();
                                    animator.Play("Close Move Object");
                                    isCanSelectToMove = true;
                                    Destroy(currentMoveArea.gameObject,0.5f);
                                }
                            }
                        }
                        if (hit.collider.gameObject.CompareTag("FirstPlayer")||hit.collider.gameObject.CompareTag("NormalPlayer"))
                        {
                            if (hit.collider.transform.childCount==2)
                            {
                                StaticObjectTouches staticObjectTouches = new StaticObjectTouches
                                {
                                    touch = Input.touches[i], currentTouchedObject = hit.collider.gameObject
                                };
                                staticObjectTouches.x = staticObjectTouches.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                                staticObjectTouches.x.drawMode = SpriteDrawMode.Tiled;
                                staticObjectTouches.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right * 10;
                                staticObjectTouches.x.size = new Vector2(staticObjectTouches.x.size.x, 20);
                                _staticObjectTouchesList.Add(staticObjectTouches);
                                break;
                            }
                        }

                        if (hit.collider.gameObject.CompareTag("RotatingPlayer"))
                        {
                            if (hit.collider.transform.childCount==3)
                            {
                                StaticObjectTouches staticObjectTouches = new StaticObjectTouches
                                {
                                    touch = Input.touches[i], currentTouchedObject = hit.collider.gameObject
                                };
                                staticObjectTouches.x = staticObjectTouches.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                                staticObjectTouches.x.drawMode = SpriteDrawMode.Tiled;
                                staticObjectTouches.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right * 10;
                                staticObjectTouches.x.size = new Vector2(staticObjectTouches.x.size.x, 20);
                                staticObjectTouches.y = staticObjectTouches.currentTouchedObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
                                staticObjectTouches.y.drawMode = SpriteDrawMode.Tiled;
                                staticObjectTouches.currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right * 10;
                                staticObjectTouches.y.size = new Vector2(staticObjectTouches.y.size.x, 20);
                                _staticObjectTouchesList.Add(staticObjectTouches);
                                break;
                            }
                        }

                        if (hit.collider.gameObject.layer.Equals(3)&&!_isDragging)
                        {
                            _isDragging = true;
                            _moveTouch.touch = Input.touches[i];
                            _moveTouch.currentTouchedObject = hit.collider.transform.parent.gameObject;
                            _oldPosition = _moveTouch.currentTouchedObject.transform.position;
                            _oldMousePosition = Input.touches[i].position;
                            if (_moveTouch.currentTouchedObject.CompareTag("NormalPlayer"))
                            {
                                _moveTouch.x = _moveTouch.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                                _moveTouch.x.drawMode = SpriteDrawMode.Tiled;
                            }
                            if (_moveTouch.currentTouchedObject.CompareTag("RotatingPlayer"))
                            {
                                _moveTouch.x = _moveTouch.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                                _moveTouch.y = _moveTouch.currentTouchedObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
                                _moveTouch.x.drawMode = SpriteDrawMode.Tiled;
                                _moveTouch.y.drawMode = SpriteDrawMode.Tiled;
                            }
                        }
                        break;
                    case TouchPhase.Moved:
                        bool isBreakable1 = false;
                        for (int j = 0; j < _staticObjectTouchesList.Count; j++)
                        {
                            if (Input.touches[i].fingerId==_staticObjectTouchesList[j].touch.fingerId)
                            {
                                isBreakable1 = true;
                                break;
                            }
                        }
                        if (isBreakable1)
                        {
                            break;
                        }
                        if (isCanSelectToMove)
                        {
                            break;
                        }
                        if (Input.touches[i].fingerId==_moveTouch.touch.fingerId&&hit.collider.gameObject.layer.Equals(3))
                        {
                            var position = _moveTouch.currentTouchedObject.transform.position;
                            position = Vector3.Lerp(position,
                                new Vector3(
                                    Mathf.Clamp(
                                        _oldPosition.x + (_camera.ScreenToWorldPoint(Input.touches[i].position) -
                                                         _camera.ScreenToWorldPoint(_oldMousePosition)).x, -9.5f, 9.5f),
                                    Mathf.Clamp(
                                        _oldPosition.y + (_camera.ScreenToWorldPoint(Input.touches[i].position) -
                                                         _camera.ScreenToWorldPoint(_oldMousePosition)).y, -5.2f, 5.2f), 0), 1f);
                            _moveTouch.currentTouchedObject.transform.position = position;
                        }
                        break;
                    case TouchPhase.Ended:
                        bool isBreakable = false;
                        for (int j = 0; j < _staticObjectTouchesList.Count; j++)
                        {
                            if (Input.touches[i].fingerId==_staticObjectTouchesList[j].touch.fingerId)
                            {
                                if (_staticObjectTouchesList[j].currentTouchedObject.CompareTag("RotatingPlayer"))
                                {
                                    _staticObjectTouchesList[j].currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                    _staticObjectTouchesList[j].currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right;
                                    _staticObjectTouchesList[j].x.size = new Vector2(_staticObjectTouchesList[j].x.size.x, 2);
                                    _staticObjectTouchesList[j].y.size = new Vector2(_staticObjectTouchesList[j].y.size.x, 2);
                                }
                                else
                                {
                                    _staticObjectTouchesList[j].currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                    _staticObjectTouchesList[j].x.size = new Vector2(_staticObjectTouchesList[j].x.size.x, 2);
                                }
                                if (isCanSelectToMove&&(_staticObjectTouchesList[j].currentTouchedObject.CompareTag("NormalPlayer")||_staticObjectTouchesList[j].currentTouchedObject.CompareTag("RotatingPlayer"))&&(hit.collider.gameObject.CompareTag("NormalPlayer")||hit.collider.gameObject.CompareTag("RotatingPlayer")))
                                {
                                    Debug.Log("Init move object");
                                    isCanSelectToMove = false;
                                    if (currentMoveArea!=null)
                                    {
                                        Destroy(currentMoveArea.gameObject);
                                        currentMoveArea = null;
                                    }
                                    currentMoveArea = Instantiate(moveObject, hit.collider.transform);
                                    currentMoveArea.transform.rotation = Quaternion.identity;
                                    currentMoveArea.transform.SetAsLastSibling();
                                }
                                _staticObjectTouchesList.RemoveAt(j);
                                isBreakable = true;
                            }
                        }

                        if (isBreakable)
                        {
                            break;
                        }
                        if (Input.touches[i].fingerId==_moveTouch.touch.fingerId)
                        {
                            _isDragging = false;
                            if (_moveTouch.currentTouchedObject.CompareTag("NormalPlayer"))
                            {
                                _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                _moveTouch.x.size = new Vector2(_moveTouch.x.size.x, 2);
                            }

                            if (_moveTouch.currentTouchedObject.CompareTag("RotatingPlayer"))
                            {
                                _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                _moveTouch.currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right;
                                _moveTouch.x.size = new Vector2(_moveTouch.x.size.x, 2);
                                _moveTouch.y.size = new Vector2(_moveTouch.y.size.x, 2);
                                _moveTouch.currentTouchedObject = null;
                                _moveTouch.touch = new Touch();
                                _moveTouch.x = null;
                                _moveTouch.y = null;
                            }
                        }
                        break;
                }
            }

            if (_isDragging)
            {
                if (_moveTouch.currentTouchedObject.CompareTag("NormalPlayer") &&
                    _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition.x < 10)
                {
                    _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition += Vector3.right * (Time.deltaTime * 10);
                    _moveTouch.x.size = new Vector2(_moveTouch.x.size.x, _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition.x * 2);
                }

                if (_moveTouch.currentTouchedObject.CompareTag("RotatingPlayer") &&
                    _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition.x < 10)
                {
                    _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition += Vector3.right * (Time.deltaTime * 10);
                    _moveTouch.currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition +=
                        Vector3.right * (Time.deltaTime * 10);
                    _moveTouch.x.size = new Vector2(_moveTouch.x.size.x, _moveTouch.currentTouchedObject.transform.GetChild(1).localPosition.x * 2);
                    _moveTouch.y.size = new Vector2(_moveTouch.y.size.x,
                        _moveTouch.currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition.x * 2);
                }
            }
        }
    }
}
