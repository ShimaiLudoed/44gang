using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour
{
 public TextMeshProUGUI dialogueText; // Ссылка на текст
    public Animator animator; // Ссылка на аниматор
    private bool isPaused = false; // Флаг паузы

    void Start()
    {
        // Устанавливаем первоначальный текст
        // Пускаем анимацию
        PlayAnimation();
    }

    void Update()
    {
        // Если игра на паузе и нажата клавиша E, продолжаем игру
        if (isPaused && Input.GetKeyDown(KeyCode.E))
        {
            ContinueGame();
        }
    }

    public void PlayAnimation()
    {
        animator.SetBool("IsAnimating", true); // Запускаем анимацию
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Останавливаем время
        isPaused = true; // Устанавливаем флаг паузы
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f; // Возобновляем время
        isPaused = false; // Сбросить флаг паузы
        // Здесь можно добавить дополнительные действия по продолжению
    }
    public void StartGame()
    {
        SceneManager.LoadScene("cutscene");
    }
     public void Game()
    {
        SceneManager.LoadScene("level");
    }
    // Метод для обновления текста при необходимости
    public void UpdateDialogueText(string newText)
    {
        dialogueText.text = newText;
    }

}
