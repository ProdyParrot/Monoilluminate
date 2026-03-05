/******************************************************************************
filename AudioMakerBGMSE.cs
author Joseph Teo
email joseph.teo@digipen.edu
Brief Description:
For the audio maker. See below for the sound library.

All content © 2017 DigiPen Institute of Technology Singapore. All Rights 
Reserved.
******************************************************************************/
using UnityEngine;

public class AudioMakerBGMSE : MonoBehaviour {

  public AudioClip[] audioList;
  public AudioSource currentAudioSource;
  public float timer;
  public bool setTimer;

  public enum BGMEnum
  {
    TITLE,
    MENU,
    LEVEL
  };

  void Start ()
  {
    currentAudioSource = GetComponent<AudioSource>();
  }

  public void PlaySound(int soundID)
  {
    currentAudioSource.clip = audioList[soundID];
    currentAudioSource.Play();
  }

  public void PlaySound(int soundID, float volume)
  {
    AdjustVolume(volume);
    currentAudioSource.clip = audioList[soundID];
    currentAudioSource.Play();
  }

  public void AdjustVolume(float vol)
  {
    currentAudioSource.volume = vol;
  }
}
