using UnityEngine;
using UnityEngine.SceneManagement;

public class WonLevelPanel : MonoBehaviour
{
    bool isLoadingScene = false;
    public static WonLevelPanel singleton;
    public GameObject initialFocusObject;
    Scene scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            scene = SceneManager.GetActiveScene();
            if(initialFocusObject!=null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(initialFocusObject);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        gameObject.SetActive(false);
        if(!isLoadingScene){
            int nextSceneIndex = scene.buildIndex+1;
            SceneManager.LoadSceneAsync(nextSceneIndex);
            isLoadingScene = true;
            Time.timeScale = 1f;
        }
        if(scene.buildIndex >= 10){
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void OnMenuLevelButtonPressed()
    {
        gameObject.SetActive(false);
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(0);
            isLoadingScene = true;
            Time.timeScale = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
