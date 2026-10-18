using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

public class textHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI texto;

    public Color preto = Color.black;
    public Color branco = Color.white;

    void Start()
    {
        texto = GetComponentInChildren<TextMeshProUGUI>();

        if (texto != null && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            texto.color = branco;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI não encontrado!", this);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (texto != null)
        {
            texto.color = preto;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (texto != null)
        {
            texto.color = branco;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (texto != null && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            texto.color = preto;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (texto != null && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            texto.color = branco;
        }
    }
}