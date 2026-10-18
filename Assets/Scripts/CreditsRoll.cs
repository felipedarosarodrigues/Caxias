using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class CreditsRoll : MonoBehaviour
{
    public RectTransform creditsText;
    public float speed = 50f;

    void Update()
    {
        creditsText.anchoredPosition +=
            Vector2.up * speed * Time.deltaTime;

        if (creditsText.position.y > creditsText.rect.height + 200f)
        {
            SceneManager.LoadScene(0);
        }
    }
}