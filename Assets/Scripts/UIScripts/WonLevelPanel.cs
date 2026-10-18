using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WonLevelPanel : MonoBehaviour
{
    bool isLoadingScene = false;
    public static WonLevelPanel singleton;
    public GameObject initialFocusObject;
    public Sprite star1, star2, star3, star4, star5;
    public Image star;
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

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            star.sprite = star1;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            star.sprite = star2;
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            star.sprite = star3;
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            star.sprite = star4;
        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            star.sprite = star5;
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
