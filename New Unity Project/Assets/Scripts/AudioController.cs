using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
   public static AudioController instance;
   public AudioClip bubbleSound,playerReceiveSound,shootSound,ballBreakSound,obstacleDetectSound,failSound,winAllLevelSound;
   public AudioSource audioSource;
   private void Awake()
   {
      instance = this;
      audioSource = GetComponent<AudioSource>();
   }

   public void PlaySound(AudioClip audioClip)
   {
      audioSource.PlayOneShot(audioClip);
      StartCoroutine(DisplayAds());
      IEnumerator DisplayAds()
      {
         yield return new WaitUntil(() => !audioSource.isPlaying);

         if (audioClip==failSound||audioClip==ballBreakSound)
         {
            if (!GameDataController.instance.removeAds)
            {
               var i = Random.Range(0, 2);
               if (i==0)
               {
                  AdsManager.instance.ShowIntestellarAds();
               }
            }
         }
      }
   }
}
