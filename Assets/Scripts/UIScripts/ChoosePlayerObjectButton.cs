using UnityEngine;

public class ChoosePlayerObjectButton : MonoBehaviour
{
    public int objectIndex = 0;
    public int playerIndex = 0;
    public GameObject objectPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            playerAffected.spawnablePlatformSlot1 = objectPrefab;
            playerAffected.LeaveSelectingObjectState();
        }
        Destroy(parentCanvas.gameObject);
    }

}
