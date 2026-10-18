using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
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
        PlayerController collidedPlayer = col.GetComponentInParent<PlayerController>();
        if (collidedPlayer != null)
        {
            GameController.singleton.coinsAmount+=1;
            Destroy(gameObject);
        }
    
    }

}
