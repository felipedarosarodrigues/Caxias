using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool isLoadingScene = false;
    [SerializeField] string StartGameSceneName = "Level 1";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGameButtonPressed()
    {
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(StartGameSceneName);
            isLoadingScene = true;
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
