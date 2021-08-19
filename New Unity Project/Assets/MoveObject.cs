using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject moveObject;
    public enum PlayerType
    {
        FirstAndStaticPlayer,
        NormalPlayer
    }

    public static GameObject currentPlayerSelected;
    public PlayerType playerType;
    private void OnMouseUp()
    {
        switch (playerType)
        {
            case PlayerType.NormalPlayer:
                if (currentPlayerSelected!=null)
                {
                    Destroy(currentPlayerSelected);
                }
                currentPlayerSelected = Instantiate(moveObject, transform);
                currentPlayerSelected.transform.SetAsLastSibling();
                currentPlayerSelected.transform.rotation = Quaternion.identity;
                break;
            case PlayerType.FirstAndStaticPlayer:
                break;
        }
    }
}
