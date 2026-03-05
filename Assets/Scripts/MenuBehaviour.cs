using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour {

    public FadeBehaviour currentFadeScreen;
    public GameObject[] starArray;

    private void Start()
    {
        currentFadeScreen.FadeIn();

        int count = 1;
        foreach(GameObject starElem in starArray)
        {
            if (CheckLevel(count) == 1)
            {
                starArray[count - 1].GetComponent<Image>().color = Vector4.one;
            }
            count++;
        }
    }

    public void LoadLevel(string levelName)
    {
        Camera.main.GetComponent<AudioSource>().Stop();
        GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.SELECT);
        StartCoroutine(loadLevelDelay(levelName));
    }

    IEnumerator loadLevelDelay(string levelName)
    {
        currentFadeScreen.FadeOut();

        // Wait 2 seconds before starting a level
        yield return new WaitForSeconds(2.0f);

        // Serialize current time
        SceneManager.LoadScene(levelName);

        // Failsafe if the level is not found
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Menu");
    }

    public int CheckLevel(int id)
    {
        string levelName = id.ToString();
        return PlayerPrefs.GetInt(levelName, 0);
    }
}
