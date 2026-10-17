using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class ChoosePlayerObjectButton : MonoBehaviour
{
    public int objectIndex = 0;
    public int playerIndex = 0;
    public GameObject objectPrefab;
    public TMP_Text currentSlotText;
    public TMP_Text objectNameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSlotText.gameObject.SetActive(false);
        if (GameController.singleton != null)
        {
            int auxIndex = math.clamp(objectIndex,0,GameController.singleton.PlatformPrefabsForPlayers.Length-1);
            objectNameText.text = GameController.singleton.PlatformPrefabsForPlayers[auxIndex].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChooseObjectButtonPressed()
    {
        print("Player "+playerIndex+ " has chosen the object "+objectIndex);
        Transform parentCanvas = transform.parent.parent.parent;
        PlayerController playerAffected = PlayerController.currentPlayerControllers[playerIndex];
        if (playerAffected != null)
        {
            playerAffected.spawnablePlatform1Index = objectIndex;
            int auxIndex = math.clamp(objectIndex,0,GameController.singleton.PlatformPrefabsForPlayers.Length-1);
            playerAffected.spawnablePlatformSlot1 = GameController.singleton.PlatformPrefabsForPlayers[auxIndex];
            playerAffected.LeaveSelectingObjectState();
        }
        Destroy(parentCanvas.gameObject);
    }

}
