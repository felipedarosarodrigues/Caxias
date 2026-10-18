using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChooseObjectPlayerCanvas : MonoBehaviour
{
    public int playerIndex = 0;
    public int slot1Index = 0;
    public int slot2Index = 0;
    public int objectsSelectedInCanvas=0;
    public Image ownerPicture;
    public Sprite catSprite;
    public Image slot1Picture;
    public Image slot2Picture;
    public Image background;
    public Sprite redBackground;
    public Sprite blueBackground;
    public Image commandUpDownImage;
    public Sprite commandUpDownRed;
    public Sprite commandUpDownBlue;
    public Image commandLeftRightImage;
    public Sprite commandLeftRightRed;
    public Sprite commandLeftRightBlue;
    public Sprite[] playerSprites;
    public RectTransform mainPanel;
    public RectTransform objectButtonsContainer;
    public  List<ChoosePlayerObjectButton> objectButtonsList;
    float lastYValue=0f;
    float lastXValue=0f;
    float timeSinceLastChange=0.2f;
    InputAction moveAction;
    InputAction buildModeAction;
    InputAction placeObjectAction;
    InputAction placeObjectAction2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        ChoosePlayerObjectButton[] buttonsArray = objectButtonsContainer.GetComponentsInChildren<ChoosePlayerObjectButton>();
        foreach (ChoosePlayerObjectButton b in buttonsArray){
            objectButtonsList.Add(b);
            b.playerIndex = playerIndex;//sets the player affected by the buttons to be the same as the canvas
        }
        ownerPicture.sprite = playerSprites[playerIndex];
        */
        if(playerIndex==0){
            moveAction = InputSystem.actions.FindAction("Movement");
            buildModeAction = InputSystem.actions.FindAction("BuildModePlayer1");
            placeObjectAction = InputSystem.actions.FindAction("InteractionButtonPlayer1");
            placeObjectAction2 = InputSystem.actions.FindAction("Interaction2ButtonPlayer1");
            background.sprite = redBackground;
            commandUpDownImage.sprite = commandUpDownRed;
            commandLeftRightImage.sprite = commandLeftRightRed;
        }
        else{
            moveAction = InputSystem.actions.FindAction("MovementPlayer2");
            buildModeAction = InputSystem.actions.FindAction("BuildModePlayer2");
            placeObjectAction = InputSystem.actions.FindAction("InteractionButtonPlayer2");
            placeObjectAction2 = InputSystem.actions.FindAction("Interaction2ButtonPlayer2");
            mainPanel.position = new Vector3(mainPanel.position.x+600,mainPanel.position.y,mainPanel.position.z);
            ownerPicture.sprite = catSprite;
            background.sprite = blueBackground;
            commandUpDownImage.sprite = commandUpDownBlue;
            commandLeftRightImage.sprite = commandLeftRightBlue;
        }
        if(playerIndex==0){
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[slot1Index];
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[slot2Index];
        }
        else{
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[slot1Index];
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[slot2Index];
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        /*if (PlayerController.currentPlayerControllers[playerIndex]!=null)
        {
            if(PlayerController.currentPlayerControllers[playerIndex].current_control_state != PlayerController.PlayerControlState.CHOOSING_OBJECT)
                gameObject.SetActive(false);
                return;
        }*/
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        //if(timeSinceLastChange>=0.2f){
        if (moveValue.y>0.2f && moveValue.y > lastYValue)
        {
            changeObjectForSlot(0,1);
        }
        else if (moveValue.y < -0.2f && moveValue.y < lastYValue)
        {
            changeObjectForSlot(0,-1);
        }

        if (moveValue.x>0.2f && moveValue.x > lastXValue)
        {
            changeObjectForSlot(1,1);
        }
        else if (moveValue.x < -0.2f && moveValue.x < lastXValue)
        {
            changeObjectForSlot(1,-1);
        }

        if (buildModeAction.triggered || placeObjectAction.triggered)
        {
            ConfirmAndClose();
        }
        //}
        timeSinceLastChange+= Time.deltaTime;
        lastYValue = moveValue.y;
        lastXValue = moveValue.x;
    }

    void changeObjectForSlot(int slot,int changeAmount)
    {
        int auxNumber=0;
        if(slot==0){
            auxNumber = slot1Index;
        }
        else{
            auxNumber = slot2Index;
        }
        auxNumber= auxNumber+ changeAmount;
        if (auxNumber >= GameController.singleton.PlatformSpritesForPlayers.Length)
        {
            auxNumber = 0;
        }
        else if (auxNumber<0)
        {
            
            auxNumber=GameController.singleton.PlatformSpritesForPlayers.Length-1;
        }
        //here we prevent if from repeating
        if( (slot==0 && auxNumber == slot2Index) || (slot!=0 && auxNumber == slot1Index))
        {
            auxNumber= auxNumber+ changeAmount;
            if (auxNumber >= GameController.singleton.PlatformSpritesForPlayers.Length)
            {
                auxNumber = 0;
            }
            else if (auxNumber<0)
            {
                auxNumber=GameController.singleton.PlatformSpritesForPlayers.Length-1;
            }
        }
        PlayerController playerAffected = PlayerController.currentPlayerControllers[playerIndex];
        if(slot==0){
            if(playerIndex==0)
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[auxNumber];
            else
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[auxNumber];
            slot1Index = auxNumber;
            playerAffected.spawnablePlatform1Index = slot1Index;
            
        }
        else{
            if(playerIndex==0)
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[auxNumber];
            else
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[auxNumber];
            slot2Index = auxNumber;
            playerAffected.spawnablePlatform2Index = auxNumber;
        }
        print("changed player's "+playerIndex+" slot"+slot+" to"+auxNumber);
        PlayerHudPanel.updateHudForPlayer(playerIndex);
        timeSinceLastChange = 0.0f;
    }

    public void ConfirmAndClose()
    {
        PlayerHudPanel.updateHudForPlayer(playerIndex);
        PlayerController.currentPlayerControllers[playerIndex].SetObjectForSlot(slot1Index,0);
        PlayerController.currentPlayerControllers[playerIndex].SetObjectForSlot(slot2Index,1);
        PlayerController.currentPlayerControllers[playerIndex].LeaveSelectingObjectState();
        gameObject.SetActive(false);
    }
}
