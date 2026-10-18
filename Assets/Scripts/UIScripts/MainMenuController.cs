using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool isLoadingScene = false;
    [SerializeField] string StartGameSceneName = "Level 1";
    [SerializeField] string Tutorial = "Tutorial 1";
    [SerializeField] GameObject[] mainPanels;
    public GameObject initialFocusObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(initialFocusObject!=null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(initialFocusObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePanel(GameObject nextPanel)
    {
        foreach(GameObject p in mainPanels)
        {
            p.SetActive(false);
        }
        nextPanel.SetActive(true);
    }

    public void SetNextFocusObject(GameObject nextFocus)
    {
        if(initialFocusObject!=null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(nextFocus);
    }

    public void OnStartGameButtonPressed()
    {
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(StartGameSceneName);
            isLoadingScene = true;
        }
    }

    public void GoToScene(int buildIndex)
    {
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(buildIndex);
            isLoadingScene = true;
        }
    }

    public void OnTutorialButtonPressed()
    {
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(Tutorial);
            isLoadingScene = true;
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
