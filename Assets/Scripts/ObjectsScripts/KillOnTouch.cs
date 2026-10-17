using UnityEngine;

public class KillOnTouch : MonoBehaviour
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
        print("DEATH Collission Collided with "+col.name+" at "+Time.timeSinceLevelLoad);
        PlayerController collidedPlayer = col.GetComponentInParent<PlayerController>();
        if (collidedPlayer != null)
        {
            collidedPlayer.Die();
        }
    
    }

}
