using UnityEngine;
using UnityEngine.UI;

public class PlayerHudPanel : MonoBehaviour
{
    public static PlayerHudPanel[] playerHudPanelsList = {null,null,null,null};
    public int playerID=0;
    public Image Slot1Image;
    public Image Slot2Image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerController.currentPlayerControllers[playerID] == null)
        {
            gameObject.SetActive(false);//hides the panel if there is no player with the same ID
        }
        else
        {
            playerHudPanelsList[playerID] = this;
            print("found HUD for the player "+playerID);
            updateSlotImages();
            
        }
    }

    void updateSlotImages()
    {
        if (PlayerController.currentPlayerControllers[playerID] != null){
            PlayerController player = PlayerController.currentPlayerControllers[playerID];
            if(player.playerID==0){
                Slot1Image.sprite = GameController.singleton.PlatformSpritesForPlayers[player.spawnablePlatform1Index];
                Slot2Image.sprite = GameController.singleton.PlatformSpritesForPlayers[player.spawnablePlatform2Index];
            }
            else
            {
                Slot1Image.sprite = GameController.singleton.PlatformSpritesForPlayer2[player.spawnablePlatform1Index];
                Slot2Image.sprite = GameController.singleton.PlatformSpritesForPlayer2[player.spawnablePlatform2Index];
            }
        }
    }

    public static void updateHudForPlayer(int index)
    {
        if (PlayerController.currentPlayerControllers[index] != null && playerHudPanelsList[index]!=null){
            PlayerController player = PlayerController.currentPlayerControllers[index];
            if(player.playerID==0){
                playerHudPanelsList[index].Slot1Image.sprite = GameController.singleton.PlatformSpritesForPlayers[player.spawnablePlatform1Index];
                playerHudPanelsList[index].Slot2Image.sprite = GameController.singleton.PlatformSpritesForPlayers[player.spawnablePlatform2Index];
            }
            else
            {
                playerHudPanelsList[index].Slot1Image.sprite = GameController.singleton.PlatformSpritesForPlayer2[player.spawnablePlatform1Index];
                playerHudPanelsList[index].Slot2Image.sprite = GameController.singleton.PlatformSpritesForPlayer2[player.spawnablePlatform2Index];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
