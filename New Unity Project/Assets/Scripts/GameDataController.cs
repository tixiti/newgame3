using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public List<PlayerData> playerData;
    public List<PortalData> portalData;
    public List<ObstacleData> obstacleData;
    public List<StarData> starData;
    public List<StaticPlayerData> staticPlayerData;
    public List<RotatingPlayerData> rotatingPlayerData;
    public List<ObstacleData> mirrorData;
}
[Serializable]
public class PlayerData
{
    public Vector rotation;
    public Vector position;
}
[Serializable]
public class RotatingPlayerData
{
    public Vector oldRotation;
    public Vector newRotation;
    public Vector position;
}
[Serializable]
public class StaticPlayerData
{
    public Vector playerRotation;
    public Vector rotation;
    public Vector position;
    public float rayLength;  
}
[Serializable]
public class PortalData
{
    public Vector positionPort1, positionPort2;
}
[Serializable]
public class ObstacleData
{
    public Vector position;
    public Vector rotation;
    public Vector scale;
};
[Serializable]
public class StarData
{
    public Vector position;
};
[Serializable]
public class Vector
{
    public float x;
    public float y;
    public float z;

    public Vector(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }
}
public class GameDataController : MonoBehaviour
{
    public static GameDataController instance;
    public GameObject Tutorials;
    [SerializeField] private GameObject Block;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
        Destroy(Block,2);
        if (PlayerPrefs.GetString("First1", "false") == "false")
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("First1", "true");
            instance.SaveData();
        }
        else
        {
            instance.LoadData();
        }
    }
    #region GameData
    public int currentLevel;
    public bool removeAdvertising;
    public int maxLevel;
    public bool boughtNewMap;
    public List<LevelData> levelData;
    #endregion
    public void SaveData()
    {
        SaveAndLoadSystem.SaveData(this);
    }

    public void LoadData()
    {
        var data = SaveAndLoadSystem.LoadData();
        currentLevel = data.currentLevel;
        levelData = data.levelData;
        maxLevel = data.maxLevel;
        boughtNewMap = data.BoughtNewMap;
        removeAdvertising = data.removeAdvertising;
    }
}
