using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOpenLevelSelector : MonoBehaviour
{
    public string mapName;

    private void OnMouseUpAsButton()
    {
        if (mapName=="MyDinh")
        {
            UIController.instance.OpenNewMap();
            return;
        }
        UIController.instance.OpenLevelSelector(mapName);
    }
}
