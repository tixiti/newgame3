using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Camera _camera;
    private GameObject _currentObject;
    private bool _isDragging = false;
    private void Awake()
    {
        _camera = Camera.main;
    }

    // Start is called before the first frame update
    void Update()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0))
            if(hit.collider != null&&hit.collider.gameObject.layer.Equals(3))
            {
                _isDragging = true;
                _currentObject = hit.collider.gameObject;
            }
        if (Input.GetMouseButton(0))
        {
            if(_isDragging)
            {
                // if (_camera.orthographicSize<8)
                // {
                //     _camera.orthographicSize += Time.deltaTime*7;
                // }
                _currentObject.transform.position = Vector3.Lerp(_currentObject.transform.position,new Vector3(Mathf.Clamp(_camera.ScreenToWorldPoint(Input.mousePosition).x,-9.5f,9.5f),Mathf.Clamp(_camera.ScreenToWorldPoint(Input.mousePosition).y/*+1*/,-5.2f,5.2f),0),0.2f);
            }
        }
//đã sửa camera zoom
//sửa k thay đổi vị trí khi chọn
        if (Input.GetMouseButtonUp(0))
            _isDragging = false;
        // if (_camera.orthographicSize>6&&!_isDragging)
        // {
        //     _camera.orthographicSize -= Time.deltaTime*10;
        // }
    }
}
