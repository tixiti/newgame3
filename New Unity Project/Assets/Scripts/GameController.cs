using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [NonSerialized] public bool isPlaying;
    [SerializeField] private GameObject ball;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject[] portalPrefabs;
    [SerializeField] private GameObject[] starPrefabs;
    [SerializeField] private GameObject[] staticPlayerPrefabs;
    [SerializeField] private GameObject[] rotatingPlayerPrefabs;
    [SerializeField] private GameObject[] mirrorPrefabs;
    [NonSerialized] public float timeStart;
    private GameDataController _gdc;
    void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        _ballRb = ball.GetComponent<Rigidbody2D>();
        ResetLevel();
    }

    private void Start()
    {
        _ballController = ball.GetComponent<BallController>();
        _BallCollider2D = ball.GetComponent<CircleCollider2D>();
        _gdc = GameDataController.instance;
    }
    private Rigidbody2D _ballRb;
    private CircleCollider2D _BallCollider2D;
    private BallController _ballController;

    private void Update()
    {
        if (isPlaying&&Time.time-timeStart>50)
        {
            StartCoroutine(DelayPushNotification());
        }
        if ((!isPlaying || !(Mathf.Abs(ball.transform.position.y) > 7)) &&
            !(Mathf.Abs(ball.transform.position.x) > 13)) return;
        StartCoroutine(DelayPushNotification());
    }

    IEnumerator DelayPushNotification()
    {
        UIController.instance.PushNotification("FAIL!");
        isPlaying = false;
        yield return new WaitForSeconds(0.5f);
        ResetLevelSoft();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void GenerateLevel(LevelData levelData)
    {
        levelText.text = "Level: " + _gdc.currentLevel;
        playerPrefabs[0].transform.eulerAngles = new Vector3(0,0,levelData.playerData[0].rotation.z);
        playerPrefabs[0].SetActive(true);
        for (var i = 1; i < levelData.playerData.Count; i++)
        {
            playerPrefabs[i].transform.position = Vector3.down*5+Vector3.right * ((-((levelData.playerData.Count+levelData.rotatingPlayerData.Count)/2)+i) * 2);
            playerPrefabs[i].transform.eulerAngles = new Vector3(levelData.playerData[i].rotation.x,levelData.playerData[i].rotation.y,levelData.playerData[i].rotation.z);
            playerPrefabs[i].SetActive(true);
        }
        for (var i = 0; i < levelData.rotatingPlayerData.Count; i++)
        {
            rotatingPlayerPrefabs[i].transform.position =  Vector3.down*5+Vector3.right * ((-((levelData.playerData.Count+levelData.rotatingPlayerData.Count)/2)+levelData.playerData.Count+i) * 2);
            rotatingPlayerPrefabs[i].transform.eulerAngles = new Vector3(0,0,RotatingPlayerController.WrapAngle(levelData.rotatingPlayerData[i].oldRotation.z));
            rotatingPlayerPrefabs[i].transform.GetChild(2).localEulerAngles = new Vector3(0,0,RotatingPlayerController.WrapAngle(levelData.rotatingPlayerData[i].newRotation.z));
            rotatingPlayerPrefabs[i].SetActive(true);
        }
        for (var i = 0; i < levelData.obstacleData.Count; i++)
        {
            obstaclePrefabs[i].transform.position = new Vector3(levelData.obstacleData[i].position.x,levelData.obstacleData[i].position.y,levelData.obstacleData[i].position.z);
            obstaclePrefabs[i].transform.eulerAngles = new Vector3(0,0,levelData.obstacleData[i].rotation.z);
            obstaclePrefabs[i].transform.localScale = new Vector3(levelData.obstacleData[i].scale.x,levelData.obstacleData[i].scale.y,levelData.obstacleData[i].scale.z);
            obstaclePrefabs[i].SetActive(true);
        }
        for (var i = 0; i < levelData.portalData.Count; i++)
        {
            portalPrefabs[i].transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
            portalPrefabs[i].transform.GetChild(1).GetComponent<CircleCollider2D>().enabled = true;
            portalPrefabs[i].transform.GetChild(0).position = new Vector3(levelData.portalData[i].positionPort1.x,levelData.portalData[i].positionPort1.y,levelData.portalData[i].positionPort1.z);
            portalPrefabs[i].transform.GetChild(1).position = new Vector3(levelData.portalData[i].positionPort2.x,levelData.portalData[i].positionPort2.y,levelData.portalData[i].positionPort2.z);
            if (levelData.portalData[i].port1Movable)
            {
                portalPrefabs[i].transform.GetChild(0).gameObject.layer = 3;
            }
            if (levelData.portalData[i].port2Movable)
            {
                portalPrefabs[i].transform.GetChild(1).gameObject.layer = 3;
            }
            portalPrefabs[i].SetActive(true);
        }

        for (var i = 0; i < levelData.starData.Count; i++)
        {
            starPrefabs[i].transform.position = new Vector3(levelData.starData[i].position.x,
                levelData.starData[i].position.y, levelData.starData[i].position.z);
        }
        for (var i = 0; i < levelData.staticPlayerData.Count; i++)
        {
            staticPlayerPrefabs[i].transform.GetChild(0).localEulerAngles = new Vector3(0,0,levelData.staticPlayerData[i].playerRotation.z);
            staticPlayerPrefabs[i].transform.position = new Vector3(levelData.staticPlayerData[i].position.x,
                levelData.staticPlayerData[i].position.y, levelData.staticPlayerData[i].position.z);
            staticPlayerPrefabs[i].transform.eulerAngles = new Vector3(0,0,levelData.staticPlayerData[i].rotation.z);
            staticPlayerPrefabs[i].transform.GetChild(0).GetComponent<StaticPlayerController>().SetRayLength(levelData.staticPlayerData[i].rayLength);
            staticPlayerPrefabs[i].SetActive(true);
        }
        for (var i = 0; i < levelData.mirrorData.Count; i++)
        {
            mirrorPrefabs[i].transform.position = new Vector3(levelData.mirrorData[i].position.x,levelData.mirrorData[i].position.y,levelData.mirrorData[i].position.z);
            mirrorPrefabs[i].transform.eulerAngles = new Vector3(0,0,levelData.mirrorData[i].rotation.z);
            mirrorPrefabs[i].transform.localScale = new Vector3(levelData.mirrorData[i].scale.x,levelData.mirrorData[i].scale.y,levelData.mirrorData[i].scale.z);
            mirrorPrefabs[i].SetActive(true);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ResetLevel()
    {
        isPlaying = false;        
        ball.GetComponent<CircleCollider2D>().enabled = false;
        ball.transform.SetParent(null);
        ball.GetComponent<BallController>().ResetShield();
        ball.transform.localScale = Vector3.one;
        _ballRb.velocity = Vector3.zero;
        ball.transform.position = Vector3.left*8;
        playerPrefabs[0].transform.eulerAngles = Vector3.left*9.3f;
        for (var i = 1; i < playerPrefabs.Length; i++)
        {
            if (!playerPrefabs[i].activeInHierarchy) continue;
            playerPrefabs[i].transform.position = Vector3.down * 20 + Vector3.left * (3 * i);
            playerPrefabs[i].SetActive(false);
        }
        for (var i = 0; i < obstaclePrefabs.Length ; i++)
        {
            if (!obstaclePrefabs[i].activeInHierarchy) continue;
            obstaclePrefabs[i].transform.position = Vector3.down * 30 + Vector3.left * (3 * i);
            obstaclePrefabs[i].SetActive(false);
        }
        for (var i = 0; i < portalPrefabs.Length ; i++)
        {
            if (!portalPrefabs[i].activeInHierarchy) continue;
            portalPrefabs[i].transform.position = Vector3.down * 40 + Vector3.left * (3 * i);
            portalPrefabs[i].SetActive(false);
        }

        for (var i = 0; i < starPrefabs.Length; i++)
        {
            if (!starPrefabs[i].activeInHierarchy) continue;
            starPrefabs[i].transform.position = Vector3.down * 50 + Vector3.left * (3 * i);
            starPrefabs[i].GetComponent<StarController>().Reset();
        }
        for (var i = 0; i < staticPlayerPrefabs.Length; i++)
        {
            if (!staticPlayerPrefabs[i].activeInHierarchy) continue;
            staticPlayerPrefabs[i].transform.position = Vector3.down * 60 + Vector3.left * (3 * i);
            staticPlayerPrefabs[i].SetActive(false);
        }
        for (var i = 0; i < rotatingPlayerPrefabs.Length; i++)
        {
            if (!rotatingPlayerPrefabs[i].activeInHierarchy) continue;
            rotatingPlayerPrefabs[i].transform.GetChild(2).gameObject.SetActive(true);
            rotatingPlayerPrefabs[i].transform.position = Vector3.down * 70 + Vector3.left * (3 * i);
            rotatingPlayerPrefabs[i].SetActive(false);
        }
        for (var i = 0; i < mirrorPrefabs.Length ; i++)
        {
            if (!mirrorPrefabs[i].activeInHierarchy) continue;
            mirrorPrefabs[i].transform.position = Vector3.down * 80 + Vector3.left * (3 * i);
            mirrorPrefabs[i].SetActive(false);
        }
    }
    public void ResetLevelSoft()
    {
        isPlaying = false;
        _BallCollider2D.enabled = false;
        ball.transform.SetParent(null);
        ball.transform.localScale = Vector3.one;
        _ballRb.velocity = Vector3.zero;
        ball.transform.position = Vector3.left*8;
        _ballController.ResetShield();
    }

    public void SaveLevelData()
    {
        var levelData = new LevelData
        {
            obstacleData = new List<ObstacleData>(),
            playerData = new List<PlayerData>(),
            portalData = new List<PortalData>(),
            starData = new List<StarData>(),
            staticPlayerData = new List<StaticPlayerData>(),
            rotatingPlayerData = new List<RotatingPlayerData>(),
            mirrorData = new List<ObstacleData>()
        };
        foreach (var t in playerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var playerData = new PlayerData {rotation = new Vector(t.transform.eulerAngles)};
            levelData.playerData.Add(playerData);
        }
        foreach (var t in rotatingPlayerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var rotatingPlayerData = new RotatingPlayerData
            {
                oldRotation = new Vector(t.transform.eulerAngles),
                newRotation = new Vector(t.transform.GetChild(2).localEulerAngles)
            };
            levelData.rotatingPlayerData.Add(rotatingPlayerData);
        }

        foreach (var t in obstaclePrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var obstacleData = new ObstacleData
            {
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                scale = new Vector(t.transform.localScale)
            };
            levelData.obstacleData.Add(obstacleData);
        }

        foreach (var t in portalPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var portalData = new PortalData
            {
                positionPort1 = new Vector(t.transform.GetChild(0).position),
                positionPort2 = new Vector(t.transform.GetChild(1).position),
                port1Movable = t.transform.GetChild(0).gameObject.layer == 3,
                port2Movable = t.transform.GetChild(1).gameObject.layer == 3
            };
            levelData.portalData.Add(portalData);
        }

        foreach (var t in starPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var starData = new StarData {position = new Vector(t.transform.position)};
            levelData.starData.Add(starData);
        }
        foreach (var t in staticPlayerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var staticPlayerData = new StaticPlayerData()
            {
                playerRotation = new Vector(t.transform.GetChild(0).localEulerAngles),
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                rayLength = t.transform.GetChild(0).GetComponent<StaticPlayerController>().playerTransform.transform.localPosition.y
            };
            levelData.staticPlayerData.Add(staticPlayerData);
        }
        foreach (var t in mirrorPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var mirrorData = new ObstacleData
            {
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                scale = new Vector(t.transform.localScale)
            };
            levelData.mirrorData.Add(mirrorData);
        }

        _gdc.levelData.Add(levelData);
        _gdc.SaveData();
    }
    public void SaveLevelData(int levelIndex)
    {
        var levelData = new LevelData
        {
            obstacleData = new List<ObstacleData>(),
            playerData = new List<PlayerData>(),
            portalData = new List<PortalData>(),
            starData = new List<StarData>(),
            staticPlayerData = new List<StaticPlayerData>(),
            rotatingPlayerData = new List<RotatingPlayerData>(),
            mirrorData = new List<ObstacleData>()
        };
        foreach (var t in playerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var playerData = new PlayerData {rotation = new Vector(t.transform.eulerAngles)};
            levelData.playerData.Add(playerData);
        }
        foreach (var t in rotatingPlayerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var rotatingPlayerData = new RotatingPlayerData
            {
                oldRotation = new Vector(t.transform.eulerAngles),
                newRotation = new Vector(t.transform.GetChild(2).localEulerAngles)
            };
            levelData.rotatingPlayerData.Add(rotatingPlayerData);
        }
        foreach (var t in obstaclePrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var obstacleData = new ObstacleData
            {
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                scale = new Vector(t.transform.localScale)
            };
            levelData.obstacleData.Add(obstacleData);
        }

        foreach (var t in portalPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var portalData = new PortalData
            {
                positionPort1 = new Vector(t.transform.GetChild(0).position),
                positionPort2 = new Vector(t.transform.GetChild(1).position),
                port1Movable = t.transform.GetChild(0).gameObject.layer == 3,
                port2Movable = t.transform.GetChild(1).gameObject.layer == 3
            };
            levelData.portalData.Add(portalData);
        }

        foreach (var t in starPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var starData = new StarData {position = new Vector(t.transform.position)};
            levelData.starData.Add(starData);
        }
        foreach (var t in staticPlayerPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var staticPlayerData = new StaticPlayerData()
            {
                playerRotation = new Vector(t.transform.GetChild(0).localEulerAngles),
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                rayLength = t.transform.GetChild(0).GetComponent<StaticPlayerController>().playerTransform.transform.localPosition.y
            };
            levelData.staticPlayerData.Add(staticPlayerData);
        }
        foreach (var t in mirrorPrefabs)
        {
            if (!t.activeInHierarchy)
                continue;
            var mirrorData = new ObstacleData
            {
                position = new Vector(t.transform.position),
                rotation = new Vector(t.transform.eulerAngles),
                scale = new Vector(t.transform.localScale)
            };
            levelData.mirrorData.Add(mirrorData);
        }
        _gdc.levelData[levelIndex] = levelData;
        _gdc.SaveData();
    }

    public void RemoveLevelData(int level)
    {
        _gdc.levelData.RemoveAt(level);
    }

    public void RenderLevel(int levelIndex)
    {
        ResetLevel();
        GenerateLevel(_gdc.levelData[levelIndex]);
    }    
}
