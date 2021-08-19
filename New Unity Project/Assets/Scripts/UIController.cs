using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] private GameObject ball;
    [SerializeField] public GameObject removeAdsBtn;
    [SerializeField] public GameObject donateBtn;
    [SerializeField] public GameObject donatePanel;
    [SerializeField] public GameObject instructionPanel;
    [SerializeField] public GameObject notificationPanel;
    [SerializeField] public Text notificationPanelText;
    [SerializeField] public GameObject hintPanel;
    [SerializeField] public GameObject instructionControl;
    [SerializeField] private GameObject notifications;
    
    [SerializeField] private GameObject statusNotification;

    [SerializeField] private TextMeshPro statusText;

    [SerializeField] private GameObject levelSelector;

    [SerializeField] private Animator mapAnimator;

    [SerializeField] private GameObject levelSelectorContent;
    [SerializeField] public GameObject settingsMenu;

    [Header("Map")]
    [SerializeField] private GameObject map;
    [SerializeField] private TextMeshPro mydinhButtonText;
    [SerializeField] private GameObject levelBtnElement;
    [SerializeField] private Sprite[] mapSprites;
    [SerializeField] private SpriteRenderer mapSpriteRenderer;
    public InputField levelToRender;
    public GameObject winPanel;
    public GameObject deadEffect;
    public ParticleSystem winEffect;
    public GameObject doneMapEffect;
    private static readonly int IsBackToMap = Animator.StringToHash("isBackToMap");
    private GameDataController _gdc;
    [SerializeField] private GameObject myDinhButtonSpriteRenderer;
    private Animator _statusNotificationAnimator;
    private static readonly int Speed = Animator.StringToHash("speed");

    public void RenderLevel()
    {
        GameController.instance.RenderLevel(int.Parse(levelToRender.text));
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _statusNotificationAnimator = statusNotification.GetComponent<Animator>();
        ChangeMapSprite(GameDataController.instance.currentLevel > 55 ?1:0);
        _gdc = GameDataController.instance;
        if (_gdc.currentLevel>55)
        {
            mapAnimator.Play("Map Anim MyDinh");
        }
        else
        {
            mapAnimator.Play("Map Anim ThongNhat");
        }
        if (_gdc.removeAds)
        {
            removeAdsBtn.SetActive(false);
            donateBtn.SetActive(true);
        }
    }

    public void PopUpNotificationPanel(string notification)
    {
        notificationPanelText.text = notification;
        notificationPanel.SetActive(true);
    }
    public void ChangeMapSprite(int mapIndex)
    {
        mapSpriteRenderer.sprite = mapSprites[mapIndex];
    }

    public void StartBtn(float speed)
    {
        GameController.instance.isPlaying = true;
        ball.GetComponent<CircleCollider2D>().enabled = true;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
    }

    public void PushNotification(string notification)
    {
        StartCoroutine(PushNotificationCoroutine(notification));

        IEnumerator PushNotificationCoroutine(string s)
        {
            statusText.text = s;
            statusNotification.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            statusNotification.SetActive(false);
        }
    }
    public void PushNotification(string notification,int time)
    {
        StartCoroutine(PushNotificationCoroutine(notification));

        IEnumerator PushNotificationCoroutine(string s)
        {
            statusText.text = s;
            statusText.GetComponent<Animator>().speed = 0.15f;
            statusNotification.SetActive(true);
            yield return new WaitForSeconds(time);
            statusText.GetComponent<Animator>().speed = 1f;
            // statusText.GetComponent<Animator>().SetFloat(Speed,1);
            statusNotification.SetActive(false);
        }
    }

    public void PopUpNotification(string message)
    {
        var clone = Instantiate(notifications,transform);
        clone.GetComponentInChildren<Text>().text = message;
    }
    #region Level Selector
    public void OpenLevelSelector(string mapName)
    {
        ResetLevelSelector();
        switch (mapName)
        {
            case "ThongNhat":
                foreach (var i in Enumerable.Range(0, 56))
                {
                    var clone = Instantiate(levelBtnElement, levelSelectorContent.transform);
                    clone.GetComponentInChildren<Text>().text = (i+1).ToString();
                    if (i<=_gdc.maxLevel)
                    {
                        clone.GetComponent<Button>().onClick.AddListener((() => OpenLevel(i)));
                    }
                    else
                    {
                        clone.GetComponent<Image>().color = Color.gray;
                        clone.GetComponent<Button>().onClick.AddListener((() => PopUpNotification("Level locked! Please play previous to unlock!")));
                    }
                }

                break;
            case "MyDinh":
                foreach (var i in Enumerable.Range(56, 18))
                {
                    var clone = Instantiate(levelBtnElement, levelSelectorContent.transform);
                    clone.GetComponentInChildren<Text>().text = i.ToString();
                    if (i<=_gdc.maxLevel)
                    {
                        clone.GetComponent<Button>().onClick.AddListener((() => OpenLevel(i)));
                    }
                    else
                    {
                        clone.GetComponent<Image>().color = Color.gray;
                        clone.GetComponent<Button>().onClick.AddListener((() => PopUpNotification("Level locked! Please play previous to unlock!")));
                    }
                }
                break;
        }
        levelSelector.SetActive(true);
    }

    public void OpenNewMap()
    {
        if (_gdc.maxLevel > 55 && _gdc.boughtNewMap)
        {
            OpenLevelSelector("MyDinh");
        }
        else if (_gdc.maxLevel<56)
        {
            PopUpNotification("My Dinh will available after you pass all Thong Nhat levels!");
        }
        else if (!_gdc.boughtNewMap)
        {
            Purchaser.instance.BuyConsumable(0);
        }
    }

    public void OpenLevel(int levelIndex)
    {
        GameController.instance.RenderLevel(levelIndex);
        levelSelector.SetActive(false);
        mapAnimator.SetBool(IsBackToMap, false);
        if (levelIndex<56)
        {
            mapAnimator.Play("Map Anim ThongNhat");
        }
        else
        {
            mapAnimator.Play("Map Anim MyDinh");
        }
    }

    public void BackToMap()
    {
        StartCoroutine(BTM());
        IEnumerator BTM()
        {
            if (_gdc.currentLevel<56)
            {
                mapAnimator.Play("BackToMapThongNhat");
                yield return new WaitForSeconds(1);
                mapAnimator.SetBool(IsBackToMap, true);
            }
            else
            {
                mapAnimator.Play("BackToMapMyDinh");
                yield return new WaitForSeconds(1);
                mapAnimator.SetBool(IsBackToMap, true);
            }
            if (_gdc.maxLevel>=56&&!_gdc.boughtNewMap)
            {
                mapAnimator.SetBool(IsBackToMap, false);
                myDinhButtonSpriteRenderer.SetActive(false);
                mydinhButtonText.text = "BUY";
                mapAnimator.Play("Done Thong Nhat Animation");
            }
            else if (_gdc.boughtNewMap)
            {
                mydinhButtonText.text = "PLAY";
                myDinhButtonSpriteRenderer.SetActive(false);
            }
        }
    }
    private void ResetLevelSelector()
    {
        for (var i = 0; i < levelSelectorContent.transform.childCount; i++)
        {
            Destroy(levelSelectorContent.transform.GetChild(i).gameObject);
        }
    }
    
    #endregion

    public void BuyNewMapSuccess()
    {
        mapAnimator.SetBool(IsBackToMap,true);
        mydinhButtonText.text = "PLAY";           
        myDinhButtonSpriteRenderer.SetActive(false);

    }
}
