using UnityEngine;

public class WonLevelPanel : MonoBehaviour
{
    public static WonLevelPanel singleton;
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
        }
    }

    public void OnNextLevelButtonPressed()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
