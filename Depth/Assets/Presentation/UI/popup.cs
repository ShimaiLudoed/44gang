using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel;
    public Button openButton;

    private Vector3 hiddenScale = Vector3.zero;
    private Vector3 shownScale = Vector3.one;
    private bool isOpen = false;

    void Start()
    {
        // Временно активируем, чтобы задать scale, потом обратно выключаем
        bool wasActive = popupPanel.activeSelf;
        popupPanel.SetActive(true);
        popupPanel.transform.localScale = hiddenScale;
        popupPanel.SetActive(wasActive);

        openButton.onClick.AddListener(OpenPopup);
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
        LeanTween.scale(popupPanel, shownScale, 0.3f).setEaseOutBack();
        isOpen = true;
    }

    void ClosePopup()
    {
        LeanTween.scale(popupPanel, hiddenScale, 0.2f).setEaseInBack().setOnComplete(() =>
        {
            popupPanel.SetActive(false);
            isOpen = false;
        });
    }
}
