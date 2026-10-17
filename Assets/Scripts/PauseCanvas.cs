using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    public static PauseCanvas singleton;
    bool isLoadingScene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            Time.timeScale = 0f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnResumeGamePressed()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void OnGoToMenuPressed()
    {
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(0);
            isLoadingScene = true;
        }
    }

    public static void PauseActionPressed()
    {
        if (singleton == null){
            GameObject pauseGameCanvas = Resources.Load("PauseCanvas") as GameObject;
            GameObject newObject = Instantiate(pauseGameCanvas);
        }
        else
        {
            singleton.OnResumeGamePressed();
        }
    }
}
