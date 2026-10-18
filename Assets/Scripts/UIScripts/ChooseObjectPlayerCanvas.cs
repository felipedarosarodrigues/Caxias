using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class ChooseObjectPlayerCanvas : MonoBehaviour
{
    public static ChooseObjectPlayerCanvas[] playerObjectCanvases = {null,null,null,null};
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
    public Image confirm;
    public Sprite confirmRed;
    public Sprite confirmBlue;
    public Sprite[] playerSprites;
    public GameObject backgroundImage;
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

    public GameObject introText;
    public float introDuration = 5f;
    private bool canChooseObject = false;
    public AudioSource countdownAudio;
    public GameObject panel;

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
            playerObjectCanvases[0] = this;
            background.sprite = redBackground;
            commandUpDownImage.sprite = commandUpDownRed;
            commandLeftRightImage.sprite = commandLeftRightRed;
            confirm.sprite = confirmRed;
            moveAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player1/Movement"];
            buildModeAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player1/BuildMode"];
            placeObjectAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player1/InteractionButton1"];
            placeObjectAction2 = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player1/InteractionButton2"];
        }
        else{
            playerObjectCanvases[1] = this;
            moveAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player2/Movement"];
            buildModeAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player2/BuildMode"];
            placeObjectAction = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player2/InteractionButton1"];
            placeObjectAction2 = PlayerController.currentPlayerControllers[playerIndex].playerInput.actions["Player2/InteractionButton2"];
            //moveAction = InputSystem.actions.FindAction("MovementPlayer2");
            //buildModeAction = InputSystem.actions.FindAction("BuildModePlayer2");
            //placeObjectAction = InputSystem.actions.FindAction("InteractionButtonPlayer2");
            //placeObjectAction2 = InputSystem.actions.FindAction("Interaction2ButtonPlayer2");
            mainPanel.SetAnchors( new Vector2(0.75f,0.5f) );
            //mainPanel.position = new Vector3(0,0,0);
            ownerPicture.sprite = catSprite;
            background.sprite = blueBackground;
            commandUpDownImage.sprite = commandUpDownBlue;
            commandLeftRightImage.sprite = commandLeftRightBlue;
            confirm.sprite = confirmBlue;
            backgroundImage.SetActive(true);

        }
        if(playerIndex==0){
                panel.SetActive(false);
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[slot1Index];
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayers[slot2Index];
        }
        else{
                slot1Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[slot1Index];
                slot2Picture.sprite = GameController.singleton.PlatformSpritesForPlayer2[slot2Index];
        }

        mainPanel.gameObject.SetActive(false);
        introText.SetActive(true);

        StartCoroutine(ShowSelectionPanel());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canChooseObject)
            return;
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

    IEnumerator ShowSelectionPanel()
    {
        // Mostra o texto inicial
        introText.SetActive(true);
        mainPanel.gameObject.SetActive(false);

        // Toca o áudio da contagem
        countdownAudio.Play();

        // Aguarda os 5 segundos
        yield return new WaitForSeconds(introDuration);

        // Esconde o texto e libera a seleção
        introText.SetActive(false);
        mainPanel.gameObject.SetActive(true);

        canChooseObject = true;
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
        playerObjectCanvases[playerIndex] = null;
        if(PlayerHudPanel.playerHudPanelsList[0]==null){
            if(playerObjectCanvases[0] == null && playerObjectCanvases[1] == null){
                GameObject spawnedHUDCanvas = Resources.Load("HUDCanvas") as GameObject;
                GameObject newHUD= Instantiate(spawnedHUDCanvas);
            }
        }
    }
}
