using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
 using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController[] currentPlayerControllers = {null,null,null,null};
    public float velocityMultiplier = 5.0f;
    public float cursorSpeed = 5.0f;
    public float cursorWobbliness = 1.0f;
    public int playerID = 0;
    public int maxAmountOfSpawnedObjects=5;
    public Rigidbody2D playerRigidbody;
    public Transform PlayerCursor;
    public Transform PlayerCursorWobblySprite;
    InputAction moveAction;
    InputAction buildModeAction;
    InputAction interactionAction;
    InputAction interaction2Action;
    InputAction pauseAction;
    float lastYValue = 0.0f;
    public enum PlayerControlState {MOVEMENT, PLACING_OBJECT, NONE, CHOOSING_OBJECT};
    public PlayerControlState current_control_state = PlayerControlState.MOVEMENT;
    public GameObject spawnablePlatformSlot1;
    public GameObject spawnablePlatformSlot2;
    public List<GameObject> spawnedPlatformsList;
    public Color platformColor;
    UnityEngine.Vector3 initialPosition = UnityEngine.Vector3.zero;

    void Awake()
    {   
        if(playerID==0)//only the player one really needs to call this
            GameController.GuaranteeSceneHasGameController();
        currentPlayerControllers[playerID] = this;
        initialPosition = transform.position;
        /*if(currentPlayerControllers==null){
            currentPlayerControllers = new List<PlayerController>();
        }
        else{
            //here we remove any index in the playerController list that is empty
            for(int i=currentPlayerControllers.Count-1; i>= 0;i--)
            {
                print("Checking if there is a player at list Index "+i);
                if (currentPlayerControllers[i] == null)
                {
                    currentPlayerControllers.RemoveAt(i);
                }
            }
        }*/
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentPlayerControllers.Add(this);
        //playerRigidbody = GetComponentInChildren<Rigidbody2D>();
        //nao é pra ser bonito, é pra funcionar rapido
        if(playerID==0){
            moveAction = InputSystem.actions.FindAction("Movement");
            buildModeAction = InputSystem.actions.FindAction("BuildModePlayer1");
            interactionAction = InputSystem.actions.FindAction("InteractionButtonPlayer1");
            interaction2Action = InputSystem.actions.FindAction("Interaction2ButtonPlayer1");
            pauseAction = InputSystem.actions.FindAction("Pause");
        }
        else{
            moveAction = InputSystem.actions.FindAction("MovementPlayer2");
            buildModeAction = InputSystem.actions.FindAction("BuildModePlayer2");
            interactionAction = InputSystem.actions.FindAction("InteractionButtonPlayer2");
            interaction2Action = InputSystem.actions.FindAction("Interaction2ButtonPlayer2");
            pauseAction = InputSystem.actions.FindAction("Pause");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        //Jump();
        PressedBuildModeAction();
        PlaceObject();
        if (playerRigidbody.position.y <= -10f)
        {
            Die();
        }
        PauseGame();
    }

    void Movement()
    {
        if(current_control_state == PlayerControlState.MOVEMENT)
        {
            UnityEngine.Vector2 moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
            playerRigidbody.linearVelocityX = moveValue.x*velocityMultiplier;
            PlayerCursor.gameObject.SetActive(false);
            
        }
        else if (current_control_state == PlayerControlState.PLACING_OBJECT)
        {
            UnityEngine.Vector2 moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
            PlayerCursor.gameObject.SetActive(true);
            PlayerCursor.position = PlayerCursor.position + new UnityEngine.Vector3(moveValue.x,moveValue.y,0) * Time.deltaTime * cursorSpeed;
            PlayerCursorWobblySprite.localPosition = new UnityEngine.Vector3(0,MathF.Cos(Time.timeSinceLevelLoad)*cursorWobbliness ,0);
            
        }
    }

    void PressedBuildModeAction()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
        {
            if(current_control_state == PlayerControlState.MOVEMENT){
                current_control_state = PlayerControlState.PLACING_OBJECT;
                PlayerCursor.position = playerRigidbody.position;
                PlayerCursor.gameObject.SetActive(true);
            }
            else if (current_control_state == PlayerControlState.PLACING_OBJECT){
                current_control_state = PlayerControlState.MOVEMENT;
                PlayerCursor.position = playerRigidbody.position;
                PlayerCursor.gameObject.SetActive(false);
            }
        }
    }

    public void LeaveSelectingObjectState()
    {
        current_control_state = PlayerControlState.MOVEMENT;
        PlayerCursor.position = playerRigidbody.position;
        PlayerCursor.gameObject.SetActive(false);
    } 

    void Jump()
    {
        if(current_control_state == PlayerControlState.MOVEMENT){
            if ( Mathf.Abs(playerRigidbody.linearVelocityY) >=0.01f)
            {
                return;
            }
            UnityEngine.Vector2 moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
            if(moveValue.y > 0.1f && moveValue.y > lastYValue)
            {
                playerRigidbody.linearVelocityY = 6.0f;
            }
            lastYValue = moveValue.y;
        }
    }

    public void Die()
    {
        transform.position = initialPosition;
        playerRigidbody.position = initialPosition;
        playerRigidbody.linearVelocity = UnityEngine.Vector2.zero;
        print("Calling Die function for player "+playerID);
    }
    void PlaceObject()
    {
        if (current_control_state == PlayerControlState.PLACING_OBJECT){
            if (interactionAction.triggered)
            {
                GameObject newObject = Instantiate(spawnablePlatformSlot1);
                newObject.transform.position = PlayerCursorWobblySprite.position;
                newObject.GetComponent<SpriteRenderer>().color = platformColor;
                spawnedPlatformsList.Add(newObject);
                if (spawnedPlatformsList.Count>maxAmountOfSpawnedObjects)
                {
                    Destroy(spawnedPlatformsList[0]);
                    spawnedPlatformsList.RemoveAt(0);
                }
                if (playerID == 0)
                {
                    newObject.layer = 10;
                }
                else
                {
                    newObject.layer = 11;
                }
            }
        }
    }

    void PauseGame()
    {
        if (pauseAction.triggered)
        {
            PauseCanvas.PauseActionPressed();
        }
    }

}
