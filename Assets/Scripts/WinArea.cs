using UnityEngine;
using UnityEngine.SceneManagement;

public class WinArea : MonoBehaviour
{
    public GameObject wonLevelPanel;
    public GameObject TutorialLevelPanel;
    public bool hasPlayer1 = false;
    public bool hasPlayer2 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("Win Collission Collided with "+col.name+" at "+Time.timeSinceLevelLoad);
        if(col.gameObject.layer==6){
            hasPlayer1 = true;
        }
        if(col.gameObject.layer==7){
            hasPlayer2 = true; 
        }
        if(hasPlayer1 && hasPlayer2){
            if (SceneManager.GetActiveScene().buildIndex <= 5)
            {
                Instantiate(wonLevelPanel);
            } else if (SceneManager.GetActiveScene().buildIndex >= 6)
            {
                Instantiate(TutorialLevelPanel);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer==6){
            hasPlayer1 = false;
        }
        if(col.gameObject.layer==7){
            hasPlayer2 = false; 
        }
    }


}
