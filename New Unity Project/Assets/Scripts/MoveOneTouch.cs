using System.Collections.Generic;
using UnityEngine;

public class MoveOneTouch : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    public struct StaticObjectTouches
    {
        public Touch touch;
        public Vector3 oldPosition;
        public Vector3 oldTouchPosition;
        public GameObject currentTouchedObject;
        public SpriteRenderer x;
        public SpriteRenderer y;
    }
    private List<StaticObjectTouches> MovingObjectList = new List<StaticObjectTouches>();
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
                        if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Movable"))
                        {
                            break;
                        }
                        if (hit.collider.transform.parent.gameObject.CompareTag("FirstPlayer")||hit.collider.transform.parent.CompareTag("NormalPlayer"))
                        {
                            var parent = hit.collider.transform.parent;
                            StaticObjectTouches staticObjectTouches = new StaticObjectTouches
                            {
                                touch = Input.touches[i], currentTouchedObject = parent.gameObject
                            };
                            staticObjectTouches.oldPosition = parent.position;
                            staticObjectTouches.oldTouchPosition = Input.touches[i].position;
                            staticObjectTouches.x = staticObjectTouches.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                            staticObjectTouches.x.drawMode = SpriteDrawMode.Tiled;
                            staticObjectTouches.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right * 10;
                            staticObjectTouches.x.size = new Vector2(staticObjectTouches.x.size.x, 20);
                            MovingObjectList.Add(staticObjectTouches);
                            break;
                        }

                        if (hit.collider.gameObject.CompareTag("RotatingPlayer"))
                        {
                            var parent = hit.collider.transform.parent;
                            StaticObjectTouches staticObjectTouches = new StaticObjectTouches
                            {
                                touch = Input.touches[i], currentTouchedObject = parent.gameObject
                            };
                            staticObjectTouches.oldPosition = parent.position;
                            staticObjectTouches.oldTouchPosition = Input.touches[i].position;
                            staticObjectTouches.x = staticObjectTouches.currentTouchedObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
                            staticObjectTouches.x.drawMode = SpriteDrawMode.Tiled;
                            staticObjectTouches.currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right * 10;
                            staticObjectTouches.x.size = new Vector2(staticObjectTouches.x.size.x, 20);
                            staticObjectTouches.y = staticObjectTouches.currentTouchedObject.transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>();
                            staticObjectTouches.y.drawMode = SpriteDrawMode.Tiled;
                            staticObjectTouches.currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right * 10;
                            staticObjectTouches.y.size = new Vector2(staticObjectTouches.y.size.x, 20);
                            MovingObjectList.Add(staticObjectTouches);
                        }
                        break;
                    case TouchPhase.Moved:
                        for (int j = 0; j < MovingObjectList.Count; j++)
                        {
                            if (Input.touches[i].fingerId==MovingObjectList[j].touch.fingerId)
                            {
                                if (MovingObjectList[j].currentTouchedObject.CompareTag("NormalPlayer")||MovingObjectList[j].currentTouchedObject.CompareTag("RotatingPlayer"))
                                {
                                    var position = MovingObjectList[j].currentTouchedObject.transform.position;
                                    position = Vector3.Lerp(position,
                                        new Vector3(
                                            Mathf.Clamp(
                                                MovingObjectList[j].oldPosition.x + (_camera.ScreenToWorldPoint(Input.touches[i].position) -
                                                                         _camera.ScreenToWorldPoint(MovingObjectList[j].oldTouchPosition)).x, -9.5f, 9.5f),
                                            Mathf.Clamp(
                                                MovingObjectList[j].oldPosition.y + (_camera.ScreenToWorldPoint(Input.touches[i].position) -
                                                                  _camera.ScreenToWorldPoint(MovingObjectList[j].oldTouchPosition)).y, -5.2f, 5.2f), 0), 1f);
                                    MovingObjectList[j].currentTouchedObject.transform.position = position;
                                }
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        for (int j = 0; j < MovingObjectList.Count; j++)
                        {
                            if (Input.touches[i].fingerId==MovingObjectList[j].touch.fingerId)
                            {
                                if (MovingObjectList[j].currentTouchedObject.CompareTag("RotatingPlayer"))
                                {
                                    MovingObjectList[j].currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                    MovingObjectList[j].currentTouchedObject.transform.GetChild(2).GetChild(0).localPosition = Vector3.right;
                                    MovingObjectList[j].x.size = new Vector2(MovingObjectList[j].x.size.x, 2);
                                    MovingObjectList[j].y.size = new Vector2(MovingObjectList[j].y.size.x, 2);
                                }
                                else
                                {
                                    MovingObjectList[j].currentTouchedObject.transform.GetChild(1).localPosition = Vector3.right;
                                    MovingObjectList[j].x.size = new Vector2(MovingObjectList[j].x.size.x, 2);
                                }
                                MovingObjectList.RemoveAt(j);
                            }
                        }
                        
                        break;
                }
            }
        }
    }
}
