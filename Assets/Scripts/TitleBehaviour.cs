using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBehaviour : MonoBehaviour
{
  public Text tapScreenImage;
  public float timer;
  public int direction;
  public float speed;
  public FadeBehaviour currentFadeScreen;
  public float loadSceneTimer;
  public bool loadScene;

    private void Awake()
    {
        //Set screen size for Standalone
        #if UNITY_STANDALONE
                Screen.SetResolution(576, 1024, false);
                Screen.fullScreen = false;
        #endif
    }
    void Update ()
    {
        timer += Time.deltaTime * direction * speed;
        if (timer > 1.0f || timer < 0.0f)
        {
            direction = -direction;
        }
        tapScreenImage.color = new Color(1.0f, 1.0f, 1.0f, timer);
        if (Input.GetButtonDown("Fire1") && !loadScene && Time.time > 1.0f)
        {
            GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.SELECT);
            speed = 10;
            currentFadeScreen.FadeOut();
            loadScene = true;
        }
        if (loadScene)
        {
            loadSceneTimer += Time.deltaTime;
        }
        if (loadSceneTimer > 1.75f)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
