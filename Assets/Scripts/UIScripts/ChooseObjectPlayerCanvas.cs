using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseObjectPlayerCanvas : MonoBehaviour
{
    public int playerIndex = 0;
    public Image ownerPicture;
    public Sprite[] playerSprites;
    public RectTransform mainPanel;
    public RectTransform objectButtonsContainer;
    public  List<ChoosePlayerObjectButton> objectButtonsList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChoosePlayerObjectButton[] buttonsArray = objectButtonsContainer.GetComponentsInChildren<ChoosePlayerObjectButton>();
        foreach (ChoosePlayerObjectButton b in buttonsArray){
            objectButtonsList.Add(b);
            b.playerIndex = playerIndex;//sets the player affected by the buttons to be the same as the canvas
        }
        ownerPicture.sprite = playerSprites[playerIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
