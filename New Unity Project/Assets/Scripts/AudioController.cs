using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
   public static AudioController instance;
   public AudioClip bubbleSound,playerReceiveSound,shootSound,ballBreakSound,obstacleDetectSound,failSound,ballFlySound;
   public AudioSource audioSource;
   private void Awake()
   {
      instance = this;
      audioSource = GetComponent<AudioSource>();
   }

   public void PlaySound(AudioClip audioClip)
   {
      audioSource.PlayOneShot(audioClip);
   }
}
