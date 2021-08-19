using System;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject miniCamera;
    [SerializeField] private GameObject miniMap;
    private GameObject _currentObject;
    private bool _isDragging;
    private Vector3 _oldPositon, _oldMousePosition;
    private RectTransform _mimiMapRectTransform;

    private void Start()
    {
        _currentObjectNull = _currentObject == null;
        _isCurrentObjectNull = _currentObject == null;
        _mimiMapRectTransform = miniMap.GetComponent<RectTransform>();
    }

    private void Awake()
    {
        _camera = Camera.main;
        if (_camera is { }) _minimapPositon = _camera.ScreenToWorldPoint(_minimapScreenPosition).x;
    }

    private readonly Vector3 _minimapScreenPosition = new Vector3(533, -20, 0);
    private float _minimapPositon;

    private SpriteRenderer _x, _y; //sprite renderer of direction
    //only x if it's normal player
    //both x,y if it's rotating player

    private bool _isCurrentObjectNull;
    private bool _currentObjectNull;

    // Start is called before the first frame update
    private void Update()
    {
        if (GameController.instance.isPlaying) return;
        // Cast a ray straight down.
        var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null && !_isDragging)
        {
            if (MoveObject.currentPlayerSelected!=null&&Input.GetMouseButtonUp(0))
            {
                Destroy(MoveObject.currentPlayerSelected);
            }
            return;}
        if (Input.GetMouseButtonDown(0))
        {
            if (MenuClickController.animator.GetCurrentAnimatorStateInfo(0).IsName("MenuBtnOpen"))
                MenuClickController.animator.Play("MenuBtnClose");
            if (hit.collider.gameObject.CompareTag("FirstPlayer"))
            {
                _isDragging = true;
                _currentObject = hit.collider.gameObject;
                _x = _currentObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                _x.drawMode = SpriteDrawMode.Tiled;
                _currentObject.transform.GetChild(1).localPosition = Vector3.right * 10;
                _x.size = new Vector2(_x.size.x, 20);
            }

            if (hit.collider.gameObject.layer.Equals(3))
            {
                _isDragging = true;
                _currentObject = hit.collider.transform.parent.gameObject;
                _oldPositon = _currentObject.transform.position;
                _oldMousePosition = Input.mousePosition;
                if (_currentObject.CompareTag("NormalPlayer"))
                {
                    _x = _currentObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                    _x.drawMode = SpriteDrawMode.Tiled;
                }

                if (_currentObject.CompareTag("RotatingPlayer"))
                {
                    _x = _currentObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                    _y = _currentObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
                    _x.drawMode = SpriteDrawMode.Tiled;
                    _y.drawMode = SpriteDrawMode.Tiled;
                }

                _mimiMapRectTransform.anchoredPosition =
                    _oldPositon.x > 0 ? new Vector2(-533, -20) : new Vector2(533, -20);
                miniMap.SetActive(true);
            }
        }

        if (Input.GetMouseButton(0))
            if (_isDragging && !_currentObject.gameObject.CompareTag("FirstPlayer"))
            {
                if (_currentObject.CompareTag("NormalPlayer") &&
                    _currentObject.transform.GetChild(1).localPosition.x < 10)
                {
                    _currentObject.transform.GetChild(1).localPosition += Vector3.right * (Time.deltaTime * 10);
                    _x.size = new Vector2(_x.size.x, _currentObject.transform.GetChild(1).localPosition.x * 2);
                }

                if (_currentObject.CompareTag("RotatingPlayer") &&
                    _currentObject.transform.GetChild(1).localPosition.x < 10)
                {
                    _currentObject.transform.GetChild(1).localPosition += Vector3.right * (Time.deltaTime * 10);
                    _currentObject.transform.GetChild(2).GetChild(0).localPosition +=
                        Vector3.right * (Time.deltaTime * 10);
                    _x.size = new Vector2(_x.size.x, _currentObject.transform.GetChild(1).localPosition.x * 2);
                    _y.size = new Vector2(_y.size.x,
                        _currentObject.transform.GetChild(2).GetChild(0).localPosition.x * 2);
                }

                var position = _currentObject.transform.position;
                position = Vector3.Lerp(position,
                    new Vector3(
                        Mathf.Clamp(
                            _oldPositon.x + (_camera.ScreenToWorldPoint(Input.mousePosition) -
                                             _camera.ScreenToWorldPoint(_oldMousePosition)).x, -9.5f, 9.5f),
                        Mathf.Clamp(
                            _oldPositon.y + (_camera.ScreenToWorldPoint(Input.mousePosition) -
                                             _camera.ScreenToWorldPoint(_oldMousePosition)).y, -5.2f, 5.2f), 0), 1f);
                _currentObject.transform.position = position;
                miniCamera.transform.position = position + Vector3.back * 100;
                if (position.x >= 6 && _mimiMapRectTransform.anchoredPosition.x > 0 && position.y > 0)
                    _mimiMapRectTransform.anchoredPosition = new Vector2(-533, -20);
                if (position.x <= -6 && _mimiMapRectTransform.anchoredPosition.x < 0 && position.y > 0)
                    _mimiMapRectTransform.anchoredPosition = new Vector2(533, -20);
            }
        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            miniMap.SetActive(false);
            _isDragging = false;
            if (_currentObject.CompareTag("NormalPlayer") || _currentObject.CompareTag("FirstPlayer"))
            {
                _currentObject.transform.GetChild(1).localPosition = Vector3.right;
                _x.size = new Vector2(_x.size.x, 2);
            }

            if (_currentObject.CompareTag("RotatingPlayer"))
            {
                _currentObject.transform.GetChild(1).localPosition = Vector3.right;
                _currentObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right;
                _x.size = new Vector2(_x.size.x, 2);
                _y.size = new Vector2(_y.size.x, 2);
            }
            
        }
        
        
    }
}