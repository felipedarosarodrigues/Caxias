using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class textHover : MonoBehaviour

{
    private TextMeshPro texto;

    public Color preto = Color.black;
    public Color branco = Color.white;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texto = GetComponent<TextMeshPro>();
        if (texto != null)
        {
            texto.color = branco;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = preto;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = branco;
        }
    }

}
