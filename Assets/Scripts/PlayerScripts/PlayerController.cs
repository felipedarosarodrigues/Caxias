using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
 using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController[] currentPlayerControllers = {null,null,null,null};
    public float velocityMultiplier = 5.0f;
    UnityEngine.Vector2 moveValue;
    public float cursorSpeed = 5.0f;
    public float cursorWobbliness = 1.0f;
    public int playerID = 0;
    public Rigidbody2D playerRigidbody;
    public Transform PlayerCursor;
    public SpriteRenderer PlayerCursorSpritePreview;
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
    public int spawnablePlatform1Index = 0;
    GameObject currentSpawnedPlatformSlot1;
    public GameObject spawnablePlatformSlot2;
    public int spawnablePlatform2Index = 0;
    GameObject currentSpawnedPlatformSlot2;
    public List<GameObject> spawnedPlatformsList;
    int whichPlatformSlotWillBePlaced = 0;
    public Color platformColor;
    UnityEngine.Vector3 initialPosition = UnityEngine.Vector3.zero;

    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool isTouchingIce=false;
    float IcyTime=0.0f;
    public bool isOnGround=false;
    public bool isOnLadder=false;
    public bool isOnRopePlatform=false;
    public ContactFilter2D raycastContactFilter2D;
    [Header("AudioParameters")]
    public AudioSource iceAudioSource;
    public AudioSource ropeAudioSource;
    public AudioSource ladderAudioSource;
    public AudioSource trampolineAudioSource;
    public AudioSource enterPlatformModeAudioSource;
    public AudioSource placePlatformAudioSource;

    void Awake()
    {   
        if(playerID==0){//only the player one really needs to call this
            GameController.GuaranteeSceneHasGameController();
            
        }
        currentPlayerControllers[playerID] = this;
        initialPosition = transform.position;
        /*if(currentPlayerControllers==null){
        if(currentPlayerControllers==null){
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

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerID == 0)
        {
            GameObject spawnedHUDCanvas = Resources.Load("HUDCanvas") as GameObject;
            GameObject newHUD= Instantiate(spawnedHUDCanvas);
        }
        if(current_control_state == PlayerControlState.CHOOSING_OBJECT){
            GameObject chooseObjectCanvas = Resources.Load("ChooseObjectPlayerCanvas") as GameObject;
            GameObject newChooseObjectCanvas= Instantiate(chooseObjectCanvas);
            newChooseObjectCanvas.GetComponent<ChooseObjectPlayerCanvas>().playerIndex = playerID;
        }

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

        if (animator != null)
        {
            bool moving = Mathf.Abs(playerRigidbody.linearVelocityX) > 0.1f;
            bool climbing = isOnLadder;

            animator.SetBool("Moving", moving && !climbing);
            animator.SetBool("Climbing", climbing);
        }
        if (playerRigidbody.linearVelocityX > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (playerRigidbody.linearVelocityX < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        checkSurfaceType();
    }

    void checkSurfaceType()
    {
        Unity.Collections.NativeArray<RaycastHit2D> raycastResults = Physics2D.Raycast(playerRigidbody.position, -UnityEngine.Vector2.up,raycastContactFilter2D,0.6f);
        isOnLadder = false;
       
        isOnGround = false;
        isOnRopePlatform = false;
        playerRigidbody.gravityScale = 1.0f;
        if (raycastResults.Length <= 0)
        {
            isOnLadder = false;
            isOnGround = false;
            return;
        }

         isTouchingIce = false;
         isOnLadder = false;

        foreach(RaycastHit2D hit in raycastResults)
        {
            isOnGround = true;
            if (hit.collider.gameObject.CompareTag("Ice"))
            {
                isTouchingIce = true;
                IcyTime = 0.5f;
                if( iceAudioSource.volume >=0.01f){
                    iceAudioSource.volume = Math.Clamp(playerRigidbody.linearVelocityX,0.0f,1.0f) ;
                }
                else
                {
                    iceAudioSource.Play();
                    iceAudioSource.volume = Math.Clamp(playerRigidbody.linearVelocityX,0.0f,1.0f) ;
                }
            }
            else if (hit.collider.gameObject.CompareTag("Ladder"))
            {
                isOnLadder = true;
                playerRigidbody.gravityScale = 0.0f;
            }
            else if (hit.collider.gameObject.CompareTag("Trampoline"))
            {
                playerRigidbody.linearVelocityY = 8f;
                if(!trampolineAudioSource.isPlaying){
                    trampolineAudioSource.pitch = UnityEngine.Random.Range(0.9f,1.1f);
                    trampolineAudioSource.Play();
                }
            }
            else if (hit.collider.gameObject.CompareTag("RopePlatform"))
            {
                isOnRopePlatform = true;
                ropeAudioSource.volume = 1.0f;
                //hit.collider.GetComponent<Rigidbody2D>().AddTorque(moveValue.x*5.0f*Time.deltaTime) ;
            }
        }
        if (!isOnRopePlatform)
        {
            ropeAudioSource.volume = Mathf.MoveTowards(ropeAudioSource.volume,0.0f,Time.deltaTime*2.0f);
        }
        if (!isTouchingIce)
        {
            iceAudioSource.volume =  Mathf.MoveTowards(iceAudioSource.volume,0.0f,Time.deltaTime* 1.0f) ;
        }
    }

    void Movement()
    {
        if(current_control_state == PlayerControlState.MOVEMENT)
        {
            moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
            if(!isTouchingIce && IcyTime <=0.001f){
                playerRigidbody.linearVelocityX = moveValue.x*velocityMultiplier;
            }
            else{
                playerRigidbody.linearVelocityX = Mathf.Clamp(playerRigidbody.linearVelocityX + moveValue.x*10.0f*Time.deltaTime , -velocityMultiplier *1.5f, velocityMultiplier*1.5f);
            }
            PlayerCursor.gameObject.SetActive(false);
            if (isOnLadder)
            {
                playerRigidbody.linearVelocityY = moveValue.y*velocityMultiplier;
                ladderAudioSource.volume = Mathf.MoveTowards(ladderAudioSource.volume, Math.Clamp(moveValue.magnitude*3.0f,0.0f,1.0f), Time.deltaTime *5.0f);
            }
            
        }
        else if (current_control_state == PlayerControlState.PLACING_OBJECT)
        {
            UnityEngine.Vector2 moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
            PlayerCursor.gameObject.SetActive(true);
            PlayerCursor.position = PlayerCursor.position + new UnityEngine.Vector3(moveValue.x,moveValue.y,0) * Time.deltaTime * cursorSpeed;
            //PlayerCursorWobblySprite.localPosition = new UnityEngine.Vector3(0,MathF.Cos(Time.timeSinceLevelLoad)*cursorWobbliness ,0);
            
        }
        if (!isOnLadder)
        {
            ladderAudioSource.volume = 0.0f;
        }
        IcyTime -=Time.deltaTime;
    }

    void PressedBuildModeAction()
    {
        if(buildModeAction==null)
            return;
        if(interactionAction.triggered || interaction2Action.triggered)
        {
            if(current_control_state == PlayerControlState.MOVEMENT){
                if(interactionAction.triggered)
                    whichPlatformSlotWillBePlaced = 0;
                else
                    whichPlatformSlotWillBePlaced = 1;
                current_control_state = PlayerControlState.PLACING_OBJECT;
                enterPlatformModeAudioSource.Play();
                PlayerCursor.position = playerRigidbody.position;
                PlayerCursor.gameObject.SetActive(true);
                if(whichPlatformSlotWillBePlaced==0){
                    if(playerID==0)
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayers[spawnablePlatform1Index];
                    else
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayer2[spawnablePlatform1Index];
                }
                else{
                    if(playerID==0)
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayers[spawnablePlatform2Index];
                    else
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayer2[spawnablePlatform2Index];
                }
            }
            else if (current_control_state == PlayerControlState.PLACING_OBJECT){
                if (interactionAction.triggered)
                {
                    whichPlatformSlotWillBePlaced =0;
                    if(playerID==0)
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayers[spawnablePlatform1Index];
                    else
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayer2[spawnablePlatform1Index];
                }
                else
                {
                    whichPlatformSlotWillBePlaced =1;
                    if(playerID==0)
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayers[spawnablePlatform2Index];
                    else
                        PlayerCursorSpritePreview.sprite = GameController.singleton.PlatformSpritesForPlayer2[spawnablePlatform2Index];
                }

                /*current_control_state = PlayerControlState.MOVEMENT;
                PlayerCursor.position = playerRigidbody.position;
                PlayerCursor.gameObject.SetActive(false);*/
            }
        }
    }

    void LeaveBuildMove()
    {
        current_control_state = PlayerControlState.MOVEMENT;
                PlayerCursor.position = playerRigidbody.position;
                PlayerCursor.gameObject.SetActive(false);
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
            if ( Mathf.Abs(playerRigidbody.linearVelocityY) <=0.01f || isOnGround)
            {
                UnityEngine.Vector2 moveValue = moveAction.ReadValue<UnityEngine.Vector2>();
                if(moveValue.y > 0.1f && moveValue.y > lastYValue)
                {
                    playerRigidbody.linearVelocityY = 6.0f;
                }
                lastYValue = moveValue.y;
            }
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
            
            if (buildModeAction.triggered)
            {
                GameObject newObject;
                if(whichPlatformSlotWillBePlaced==0)
                    newObject = Instantiate(spawnablePlatformSlot1);
                else
                    newObject = Instantiate(spawnablePlatformSlot2);
                newObject.transform.position = PlayerCursorWobblySprite.position;
                newObject.GetComponent<SpriteRenderer>().color = platformColor;
                if(whichPlatformSlotWillBePlaced==0){
                    if(currentSpawnedPlatformSlot1!=null)
                        Destroy(currentSpawnedPlatformSlot1.gameObject);
                    currentSpawnedPlatformSlot1 = newObject;
                }
                else
                {
                    if(currentSpawnedPlatformSlot2!=null)
                        Destroy(currentSpawnedPlatformSlot2.gameObject);
                    currentSpawnedPlatformSlot2 = newObject;
                }
                /*spawnedPlatformsList.Add(newObject);
                if (spawnedPlatformsList.Count>=6)
                {
                    Destroy(spawnedPlatformsList[0]);
                    spawnedPlatformsList.RemoveAt(0);
                }*/
                if (playerID == 0)
                {
                    newObject.layer = 10;
                }
                else
                {
                    newObject.layer = 11;
                }
                placePlatformAudioSource.Play();
                LeaveBuildMove();
            }
            
        }
        
    }

    public void SetObjectForSlot(int objectIndex, int slot)
    {
        if (slot == 0)
        {
            spawnablePlatform1Index = objectIndex;
            spawnablePlatformSlot1 = GameController.singleton.PlatformPrefabsForPlayers[objectIndex];
        }
        else
        {
            spawnablePlatform2Index = objectIndex;
            spawnablePlatformSlot2 = GameController.singleton.PlatformPrefabsForPlayers[objectIndex];
        }
    }

    void PauseGame()
    {
        if(pauseAction==null)
            return;
        if (pauseAction.triggered)
        {
            PauseCanvas.PauseActionPressed();
        }
    }

}
