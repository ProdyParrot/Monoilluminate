using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBehaviour : MonoBehaviour {

    public float timer;
    public Image currentImage;
    public bool activated;
    public bool activated2;

    // Use this for initialization
    void Start () {
        currentImage = GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
		if (activated)
        {
            timer += Time.deltaTime;
            SetAlpha(timer);
        }
        if (activated2)
        {
            timer -= Time.deltaTime;
            SetAlpha(timer);
        }
    }

    public void FadeOut()
    {
        activated2 = false;
        timer = 0.0f;
        activated = true;
    }

    public void FadeIn()
    {
        activated = false;
        timer = 1.0f;
        activated2 = true;
    }

    public void SetAlpha(float inputAlpha)
    {
        currentImage.color = new Color(0.0f, 0.0f, 0.0f, inputAlpha);
    }
}
