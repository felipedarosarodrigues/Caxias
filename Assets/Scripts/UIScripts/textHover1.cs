using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class textHover1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI texto;

    public Color preto = Color.black;
    public Color branco = Color.white;

    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();

        if (texto != null)
        {
            texto.color = preto;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI não encontrado!", this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = branco;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = preto;
        }
    }
}