using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    private bool isPaused = false;

    void Start()
    {
        PlayAnimation();
    }

    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.E))
        {
            ContinueGame();
        }
    }

    public void PlayAnimation()
    {
        animator.SetBool("IsAnimating", true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("cutscene");
    }

    public void Game()
    {
        SceneManager.LoadScene("level");
    }

    public void UpdateDialogueText(string newText)
    {
        dialogueText.text = newText;
    }
}
