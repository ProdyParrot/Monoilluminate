using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnButtonBehaviour : MonoBehaviour
{
    float timer;
    bool confirm;
    public Text buttonText;
    public FadeBehaviour currentFadeScreen;
    private GameLogic gameRef;

    private void Start()
    {
        currentFadeScreen = GameObject.Find("FadeObject").GetComponent<FadeBehaviour>();
        gameRef = GameObject.Find("GameEngine").GetComponent<GameLogic>();
    }

    void Update()
    {
        if (confirm)
        {
            buttonText.text = "Confirm";
            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                confirm = false;
                timer = 0.0f;
            }
        }
        else
        {
            buttonText.text = "Menu";
        }
    }

    public void pushButton()
    {
        if (!confirm)
        {
            confirm = true;
        }
        else
        {
            StartCoroutine(loadLevelDelay("Menu"));
        }
    }
    IEnumerator loadLevelDelay(string levelName)
    {
        currentFadeScreen.FadeOut();
        gameRef.PlaySound(0);

        // Wait 2 seconds before starting a level
        yield return new WaitForSeconds(2.0f);

        // Serialize current time
        SceneManager.LoadScene(levelName);

        // Failsafe if the level is not found
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Menu");
    }

}
