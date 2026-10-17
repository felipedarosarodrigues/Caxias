using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController singleton;
    public int coinsAmount=0;
    public GameObject[] PlatformPrefabsForPlayers;
    public Sprite[] PlatformSpritesForPlayers;
    //this is the autoload that mostly holds a bunch of data
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //function that creates a game controller on scenes that don't have it
    public static void GuaranteeSceneHasGameController()
    {
        if(singleton==null){
            GameObject spawnedGameController = Resources.Load("GameController") as GameObject;
            GameObject newObject = Instantiate(spawnedGameController);
            print("created a GameController for a scene that didn't have it yet");
        }
    }

}
