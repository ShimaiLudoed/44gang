using UnityEngine;
using UnityEngine.UI;
using static LeanTween;
public class PopupController : MonoBehaviour
{
    public GameObject popupPanel;
    public Button openButton;

    private Vector3 hiddenScale = Vector3.zero;
    private Vector3 shownScale = Vector3.one;
    private bool isOpen = false;

    void Start()
    {
        popupPanel.transform.localScale = hiddenScale;
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
