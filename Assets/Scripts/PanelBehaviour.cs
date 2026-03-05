using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    Button currButton;
    public int currPiece;
    private GameLogic gameRef;
    public Image currSprite;
    public bool buttonPressed;
    public bool fixedPanel;
    public PanelBehaviour upPanel;
    public PanelBehaviour rightPanel;
    public PanelBehaviour downPanel;
    public PanelBehaviour leftPanel;

    // Start is called before the first frame update
    void Start()
    {
        currButton = GetComponent<Button>();
        gameRef = GameObject.Find("GameEngine").GetComponent<GameLogic>();
        DisplayImage();
        if (fixedPanel)
        {
            currButton.interactable = false;
        }

        if (upPanel && !upPanel.isActiveAndEnabled)
        {
            upPanel = null;
        }
        if (rightPanel && !rightPanel.isActiveAndEnabled)
        {
            rightPanel = null;
        }
        if (downPanel && !downPanel.isActiveAndEnabled)
        {
            downPanel = null;
        }
        if (leftPanel && !leftPanel.isActiveAndEnabled)
        {
            leftPanel = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PieceTransfer(PanelBehaviour otherPanel)
    {
        if (otherPanel != null)
        {
            int myID = currPiece;
            currPiece = otherPanel.currPiece;
            otherPanel.currPiece = myID;
            otherPanel.DisplayImage();
        }
        DisplayImage();
    }

    int PopItem()
    {
        int itemToReturn = currPiece;
        currPiece = 0;
        return itemToReturn;
    }

    void AddItem(int pieceToAdd)
    {
        currPiece = pieceToAdd;
    }

    void DisplayImage()
    {
        if (currPiece == 0)
        {
            currSprite.gameObject.SetActive(false);
        }
        else
        {
            currSprite.gameObject.SetActive(true);
            currSprite.sprite = gameRef.GetPuzzleSprite(currPiece);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameLogic.executing || fixedPanel) return;

        if (currPiece != 0)
        {
            gameRef.PlaySound(1);
            GameLogic.draggingObject = true;
            gameRef.MouseSpriteImage(currPiece);
            currSprite.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameLogic.executing || fixedPanel) return;

        if (GameLogic.draggingObject && GameLogic.currentMouseOver != null && GameLogic.currentMouseOver != this && !GameLogic.currentMouseOver.fixedPanel)
        {
            // Transfer piece to another panel
            gameRef.PlaySound(2);
            PieceTransfer(GameLogic.currentMouseOver);
        }
        else if (currPiece > 0)
        {
            currSprite.gameObject.SetActive(true);
        }
        GameLogic.draggingObject = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        GameLogic.currentMouseOver = this;
        if (!gameRef.levelComplete && currPiece > 0)
        {
            gameRef.UpdateDesc();
        }
        EventSystem.current.SetSelectedGameObject(GameLogic.currentMouseOver.gameObject);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (GameLogic.currentMouseOver == this)
        {
            GameLogic.currentMouseOver = null;
        }
    }
}
