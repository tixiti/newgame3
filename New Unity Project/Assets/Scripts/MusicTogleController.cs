using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTogleController : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetString("MuteSound")=="True")
        {
            AudioController.instance.audioSource.mute = true;
            ToggleSoundEffect();
        }
        else AudioController.instance.audioSource.mute = false;
    }

    public void ToggleSoundEffect()
    {
        if (AudioController.instance.audioSource.mute)
        {
            GetComponent<Toggle>().isOn = true;
            PlayerPrefs.SetString("MuteSound","False");
        }
        else
        {
            GetComponent<Toggle>().isOn = false;
            PlayerPrefs.SetString("MuteSound","True");
        }
        AudioController.instance.audioSource.mute = !AudioController.instance.audioSource.mute;
    }
}
