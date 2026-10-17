using UnityEngine;

public class ChangeSpriteBasedOnLayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite spriteForPlayer2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(gameObject.layer == 11)
            spriteRenderer.sprite = spriteForPlayer2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
