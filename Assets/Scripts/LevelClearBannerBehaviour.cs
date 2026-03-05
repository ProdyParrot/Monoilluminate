using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearBannerBehaviour : MonoBehaviour
{
    private Vector2 destinationPos;
    public bool activated;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        destinationPos = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            GetComponent<RectTransform>().anchoredPosition += (destinationPos - GetComponent<RectTransform>().anchoredPosition) * Time.deltaTime * speed;
        }
    }
}
