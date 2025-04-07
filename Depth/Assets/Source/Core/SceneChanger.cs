using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
  public void StartGame()
  {
    SceneManager.LoadScene("bar");
  }
  public void QuitGame()
  {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
  }
}
