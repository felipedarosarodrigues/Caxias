using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController singleton;
    public int coinsAmount=0;
    public GameObject[] PlatformPrefabsForPlayers;
    public Sprite[] PlatformSpritesForPlayers;
    public bool[] unlockedLevels = new bool[9];
    public int[] coinsCollectedByLevel = new int[9];
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

    public static void addCoin()
    {
        if (singleton == null)
            return;
        singleton.coinsAmount+=1;
        Scene scene = SceneManager.GetActiveScene();
        int currentSceneIndex = scene.buildIndex;
        singleton.coinsCollectedByLevel[currentSceneIndex-1]+=1;

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
