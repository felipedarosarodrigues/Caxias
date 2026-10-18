using Unity.Mathematics;
using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    public static SceneMusicController singleton;
    public bool isFadingOut;
    public AudioSource currentMusic;
    public float fadedoutSpeed=2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(singleton.currentMusic.clip == currentMusic.clip)
            {
                Destroy(gameObject);//se a sua musica ja está tocando, só se destrua e nao comece a tocar
            }
            else //se a musica é diferente, dê fadedout na musica antiga e pegue o papel de singleton como nova musica atual
            {
                singleton.BeginFadeout();
                singleton  = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public void BeginFadeout()
    {
        isFadingOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingOut)
        {
            currentMusic.volume = Mathf.MoveTowards(currentMusic.volume,0.0f,Time.deltaTime*fadedoutSpeed);
            if(currentMusic.volume <= 0.01f)
            {
                Destroy(gameObject);//se destrua ao terminar de fazer o fadeout
            }
        }
    }
}
