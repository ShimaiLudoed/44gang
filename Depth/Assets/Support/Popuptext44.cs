using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterTrigger : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public string fullText = "Добро пожаловать в зону...";
    public float typeSpeed = 0.05f;
    public float delayBeforeErase = 2f;
    public float eraseSpeed = 0.03f;

    private bool hasActivated = false;

    private void Start()
    {
        if (textUI != null)
            textUI.gameObject.SetActive(false); // Отключаем текст в начале
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            StartCoroutine(ShowTextRoutine());
        }
    }

    IEnumerator ShowTextRoutine()
    {
        textUI.gameObject.SetActive(true); // Включаем текст
        textUI.text = "";

        // Печатание по буквам
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Ждём перед исчезновением
        yield return new WaitForSeconds(delayBeforeErase);

        // Удаление по буквам
        while (textUI.text.Length > 0)
        {
            textUI.text = textUI.text.Substring(0, textUI.text.Length - 1);
            yield return new WaitForSeconds(eraseSpeed);
        }

        textUI.gameObject.SetActive(false); // Выключаем после удаления
    }
}
