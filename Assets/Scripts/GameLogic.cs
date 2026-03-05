using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public string levelName;
    public string nextLevelName;
    public int selectedPiece;
    public static PanelBehaviour selectedPanel;
    public static bool draggingObject;
    public static PanelBehaviour currentMouseOver;
    public Sprite[] pieceSpriteLib;
    public Image spriteDragImage;
    public static bool executing;
    public Text descText;
    public bool levelComplete;
    public GameObject[] boardPanelArray;
    public GameObject[] itemPanelArray;
    public GameObject lightPointerObject;
    public List<GameObject> lightParticles;
    public int goalQuota = 0;
    public int levelQuota;
    private int lightProcessed;
    public Text exeButtonText;
    public Button exeButton;
    private bool lightOnBoard;
    public GameObject currCanvas;
    public FadeBehaviour currFadeScreen;
    public LevelClearBannerBehaviour levelCompleteBannerObj;

    private bool loadScene;
    private float loadSceneTimer = 0.0f;

    void Start()
    {
        selectedPanel = null;
        currentMouseOver = null;
        executing = false;
        draggingObject = false;
        goalQuota = 0;
        boardPanelArray = GameObject.FindGameObjectsWithTag("BoardPanel");
        itemPanelArray = GameObject.FindGameObjectsWithTag("ItemPanel");
        descText.text = "";
        currFadeScreen.FadeIn();

        int levelID = int.Parse(levelName);
        levelID++;
        if (levelID < 12)
        {
            nextLevelName = "Level";
            nextLevelName += levelID.ToString();
        }
        else
        {
            nextLevelName = "Menu";
        }
    }

    void Update()
    {
        if (loadScene)
        {
            loadSceneTimer += Time.deltaTime;
        }
        if (loadSceneTimer > 1.75f)
        {
            SceneManager.LoadScene(nextLevelName);
        }

        if (draggingObject)
        {
            spriteDragImage.gameObject.SetActive(true);
            MouseSpritePos();
        }
        else
        {
            spriteDragImage.gameObject.SetActive(false);
        }

        if (levelComplete)
        {
            descText.fontStyle = FontStyle.Bold;
            descText.text = "Level Completed!";
        }
    }

    void MouseSpritePos()
    {
        Vector2 mousePos = Input.mousePosition;
        float xPos = mousePos.x / Screen.width * currCanvas.GetComponent<RectTransform>().rect.width;
        float yPos = mousePos.y / Screen.height * currCanvas.GetComponent<RectTransform>().rect.height;
        spriteDragImage.rectTransform.anchoredPosition = new Vector2(xPos, yPos);
    }

    public void MouseSpriteImage(int id)
    {
        spriteDragImage.sprite = GetPuzzleSprite(id);
    }

    public Sprite GetPuzzleSprite(int id)
    {
        return pieceSpriteLib[id];
    }

    public void ExecutePuzzle()
    {
        if (levelComplete)
        {
            if (!loadScene)
            {
                PlaySound(0);
                currFadeScreen.FadeOut();
            }
            loadScene = true;
            return;
        }

        if (!lightOnBoard)
        {
            lightOnBoard = true;
            bool lightFound = false;
            foreach (GameObject panelElem in boardPanelArray)
            {
                PanelBehaviour panelScript = panelElem.GetComponent<PanelBehaviour>();
                if (panelScript.currPiece == 1)
                {
                    GameObject lightObj = Instantiate(lightPointerObject, panelElem.transform.position, Quaternion.identity, panelElem.transform.parent);
                    lightObj.GetComponent<LightBehaviour>().nextPanel = panelScript.rightPanel;
                    lightFound = true;
                }
            }
            if (!lightFound)
            {
                lightOnBoard = false;
                descText.text = "No Light Generators found on the board.";
                return;
            }
            foreach (GameObject panelElem in itemPanelArray)
            {
                PanelBehaviour panelScript = panelElem.GetComponent<PanelBehaviour>();
                if (panelScript.currPiece == 1)
                {
                    lightProcessed++;
                }
            }
            goalQuota = 0;
            exeButton.interactable = false;
            exeButtonText.text = "Executing...";
            executing = true;
            PlaySound(0);
        }
        else
        {
            foreach (GameObject lightParticleObj in lightParticles)
            {
                Destroy(lightParticleObj);
            }
            lightOnBoard = false;
            exeButtonText.text = "Execute";
            executing = false;
        }
    }

    public void CheckQuota()
    {
        lightProcessed++;
        if (goalQuota == levelQuota)
        {
            LevelComplete();
            return;
        }
        if (lightProcessed == levelQuota)
        {
            lightProcessed = 0;
            descText.text = "All Goal Pieces did not recieve light. Try again!";
            exeButton.interactable = true;
            exeButtonText.text = "Revert";
        }
    }

    public void LevelComplete()
    {
        levelCompleteBannerObj.activated = true;
        PlayerPrefs.SetInt(levelName, 1);
        descText.text = "Level Complete!";
        exeButton.interactable = true;
        exeButtonText.text = "Go to Next Level";
        levelComplete = true;
    }

    public void UpdateDesc()
    {
        descText.text = GetDesc();
    }

    public string GetDesc()
    {
        if (currentMouseOver != null)
        {
            return ItemDescriptions(currentMouseOver.currPiece);
        }
        else
        {
            return ItemDescriptions(0);
        }
    }

    public static string ItemDescriptions(int id)
    {
        switch (id)
        {
            case 0:
                return "";
            case 1:
                return "Light Generator\nGenerates light forward which must reach a goal piece.";
            case 2:
                return "Gear Piece (R)\nCauses light to bend clockwise when it goes through this piece.";
            case 3:
                return "Gear Piece (L)\nCauses light to bend anti-clockwise when it goes through this piece.";
            case 4:
                return "Magnifier\nIncreases the intensity of light, allowing it to pass through acrylic blocks for one time.";
            case 5:
                return "Acrylic Block\nBlocks light unless it has passed through a magnifier at least once.";
            case 6:
                return "Goal Piece\nAll Goal Pieces must receive light for the level to be cleared.";
            default:
                return "";
        }
    }

    public void PlaySound(int id)
    {
        switch (id)
        {
            case 0:
                GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.SELECT);
                break;
            case 1:
                GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.PICK);
                break;
            case 2:
                GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.PLACE);
                break;
            case 3:
                GetComponent<SoundManager>().PlaySound(transform.position, (int)AudioMakerSFX.SoundEnum.HIT);
                break;
        }
    }
}
