using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    bool collected = false;
    public GameObject onDestroyedSpawn; 
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
        if(collected)//don't do anything if it has already been collected
            return;
        PlayerController collidedPlayer = col.GetComponentInParent<PlayerController>();
        if (collidedPlayer != null)
        {
            collected = true;
            GameController.addCoin();
            Destroy(gameObject);
        }
    
    }

    void OnDestroy()
    {
        if(onDestroyedSpawn!=null){
            GameObject aux = Instantiate(onDestroyedSpawn);
            Destroy(aux,1.0f);
        }
    }

}
