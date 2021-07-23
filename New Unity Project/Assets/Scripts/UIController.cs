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
    
    [SerializeField] private GameObject statusNotification;

    [SerializeField] private TextMeshPro statusText;

    [SerializeField] private GameObject levelSelector;

    [SerializeField] private Animator mapAnimator;

    [SerializeField] private GameObject levelSelectorContent;

    [SerializeField] private GameObject levelBtnElement;
    private static readonly int IsBackToMap = Animator.StringToHash("isBackToMap");

    private void Awake()
    {
        instance = this;
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

    #region MyRegion

    public void OpenLevelSelector(string mapName)
    {
        ResetLevelSelector();
        switch (mapName)
        {
            case "ThongNhat":
                foreach (var i in Enumerable.Range(0, 56))
                {
                    var clone = Instantiate(levelBtnElement, levelSelectorContent.transform);
                    clone.GetComponentInChildren<Text>().text = i.ToString();
                    clone.GetComponent<Button>().onClick.AddListener((() => OpenLevel(i)));
                }

                break;
            case "MyDinh": 
                foreach (var i in Enumerable.Range(56, 18))
                {
                    var clone = Instantiate(levelBtnElement, levelSelectorContent.transform);
                    clone.GetComponentInChildren<Text>().text = i.ToString();
                    clone.GetComponent<Button>().onClick.AddListener((() => OpenLevel(i)));
                }
                break;
        }
        levelSelector.SetActive(true);
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
        mapAnimator.SetBool(IsBackToMap, true);
    }
    private void ResetLevelSelector()
    {
        for (var i = 0; i < levelSelectorContent.transform.childCount; i++)
        {
            Destroy(levelSelectorContent.transform.GetChild(i).gameObject);
        }
    }
    
    #endregion
}
