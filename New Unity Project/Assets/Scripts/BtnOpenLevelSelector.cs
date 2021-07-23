using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOpenLevelSelector : MonoBehaviour
{
    public string mapName;

    private void OnMouseUpAsButton()
    {
        UIController.instance.OpenLevelSelector(mapName);
    }
}
