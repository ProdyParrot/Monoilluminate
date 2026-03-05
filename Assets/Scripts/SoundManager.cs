using UnityEngine;

public class SoundManager : MonoBehaviour {

    public GameObject audioMakerObjSE;
    public GameObject audioMakerObjBGM;

    public void PlaySound(Vector3 location, int soundID)
    {
      GameObject soundObj = Instantiate(audioMakerObjSE, location, Quaternion.identity);
      soundObj.GetComponent<AudioMakerSFX>().PlaySound(soundID);
    }

    public void PlaySound(Vector3 location, int soundID, float volume)
    {
        GameObject soundObj = Instantiate(audioMakerObjSE, location, Quaternion.identity);
        soundObj.GetComponent<AudioMakerSFX>().PlaySound(soundID, volume);
    }

    public void PlayBGM(int inputTrack)
    {
        audioMakerObjBGM.GetComponent<AudioMakerBGMSE>().PlaySound(inputTrack);
    }
}
