using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshPro;
    private Color originalColor;

    void Awake()
    {
        // Ищем TextMeshProUGUI среди дочерних
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI не найден в дочерних объектах " + gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = Color.black;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = originalColor;
        }
    }
}
