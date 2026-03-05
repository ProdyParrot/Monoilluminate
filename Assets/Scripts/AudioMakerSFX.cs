using UnityEngine;

public class AudioMakerSFX : MonoBehaviour {

  public AudioClip[] audioList;
  public AudioSource currentAudioSource;

  public enum SoundEnum
  {
    SELECT,
    PICK,
    PLACE,
    HIT
  };

  void Start ()
  {
    currentAudioSource = GetComponent<AudioSource>();
  }

  public void PlaySound(int soundID)
  {
    currentAudioSource.clip = audioList[soundID];
    currentAudioSource.Play();
    Destroy(gameObject, audioList[soundID].length);
  }

  public void PlaySound(int soundID, float volume)
  {
    currentAudioSource.clip = audioList[soundID];
    AdjustVolume(volume);
    currentAudioSource.Play();
    Destroy(gameObject, audioList[soundID].length);
  }

  public void AdjustVolume(float vol)
  {
    currentAudioSource.volume = vol;
  }

  public bool isPlaying()
  {
    return currentAudioSource.isPlaying;
  }
}
