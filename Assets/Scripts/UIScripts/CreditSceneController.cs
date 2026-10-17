using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSceneController : MonoBehaviour
{
    bool isLoadingScene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnBackToMenuButtonPressed()
    {
        gameObject.SetActive(false);
        if(!isLoadingScene){
            SceneManager.LoadSceneAsync(0);
        }
    }

}
