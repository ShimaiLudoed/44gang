using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class tutor : MonoBehaviour
{
    public GameObject popupPanel;
    public GameObject darkBackground;
    public GameObject[] popupItems;

    public Button openButton;

    private bool isOpen = false;
    private float openDelay = 0.5f;
    private float closeDelay = 0.2f;

    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        popupPanel.SetActive(false);

        openButton.onClick.AddListener(OpenPopup);

        // Сохраняем оригинальные скейлы и прячем айтемы
        foreach (var item in popupItems)
        {
            originalScales[item] = item.transform.localScale;
            item.transform.localScale = Vector3.zero;
        }

        // Прозрачный фон
        darkBackground.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
    }

    void OpenPopup()
    {
        popupPanel.SetActive(true);
        isOpen = true;

        CanvasGroup bgGroup = darkBackground.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(bgGroup, 1f, 0.3f);

        for (int i = 0; i < popupItems.Length; i++)
        {
            int index = i;
            GameObject item = popupItems[index];
            item.transform.localScale = Vector3.zero;

            LeanTween.delayedCall(openDelay * index, () =>
            {
                LeanTween.scale(item, originalScales[item], 0.4f).setEaseOutBack();
            });
        }
    }

    void ClosePopup()
    {
        isOpen = false;

        for (int i = popupItems.Length - 1; i >= 0; i--)
        {
            int index = i;
            GameObject item = popupItems[index];

            LeanTween.delayedCall(closeDelay * (popupItems.Length - 1 - index), () =>
            {
                LeanTween.scale(item, Vector3.zero, 0.2f).setEaseInBack();
            });
        }

        LeanTween.delayedCall(closeDelay * popupItems.Length + 0.1f, () =>
        {
            CanvasGroup bgGroup = darkBackground.GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(bgGroup, 0f, 0.2f).setOnComplete(() =>
            {
                popupPanel.SetActive(false);
            });
        });
    }
}
