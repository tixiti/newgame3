using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class MenuClickController : MonoBehaviour
{
    [Serializable]
    public enum ButtonName
    {
        Settings,
        Menu,
        Hint,
        Replay,
        Instructions,
        Map,
        RemoveAds,
        Donate
    }

    private void Start()
    {
        animator = GameObject.Find("MenuBtn").GetComponent<Animator>();
    }

    public static Animator animator;
    public ButtonName buttonName;
    private void OnMouseDown()
    {
        switch (buttonName)
        {
            case ButtonName.Hint:
                if (HintController.instance.currentHint>=GameDataController.instance.levelData[GameDataController.instance.currentLevel].playerData.Count-1+GameDataController.instance.levelData[GameDataController.instance.currentLevel].rotatingPlayerData.Count)
                {
                    UIController.instance.PopUpNotificationPanel("No hint more!");
                }
                else
                {
                    UIController.instance.hintPanel.SetActive(true);
                }
                break;
            case ButtonName.Settings:
                UIController.instance.settingsMenu.SetActive(true);
                animator.Play("MenuBtnClose");
                break;
            case ButtonName.Menu:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("MenuBtnOpen"))
                {
                    animator.Play("MenuBtnClose");
                }
                else
                {
                    animator.Play("MenuBtnOpen");
                }
                break;
            case ButtonName.Instructions:
                UIController.instance.instructionPanel.SetActive(true);
                animator.Play("MenuBtnClose");
                break;
            case ButtonName.Map:
                UIController.instance.BackToMap();
                animator.Play("MenuBtnClose");
                break;
            case ButtonName.Replay:
                GameController.instance.RenderLevel(GameDataController.instance.currentLevel);
                break;
            case ButtonName.RemoveAds:
                Purchaser.instance.RemoveAds();
                break;
            case ButtonName.Donate:
                UIController.instance.donatePanel.SetActive(true);
                break;
        }
    }
}
