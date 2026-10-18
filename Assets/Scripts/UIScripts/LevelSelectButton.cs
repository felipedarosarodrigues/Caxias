using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public UnityEngine.UI.Image starsImage;
    public Sprite[] starsSpritesByAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        int amountOfCoins = GameController.singleton.coinsCollectedByLevel[levelIndex];
        amountOfCoins = Math.Clamp(amountOfCoins,0,5);
        starsImage.sprite = starsSpritesByAmount[amountOfCoins];
    }

}
