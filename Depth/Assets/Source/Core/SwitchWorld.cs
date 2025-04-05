using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchWorld : MonoBehaviour
{
  public event Action OnChange;
  private Vector3 _playerPosition;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.E)) 
    {
      ChangeWorld();
    }
  }

  private void ChangeWorld()
  {
    _playerPosition = transform.position;
    if (SceneManager.GetActiveScene().name == "Norm")
    {
      SceneManager.LoadScene("Curse");
    }
    else
    {
      SceneManager.LoadScene("Norm");
    }
    
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    GameObject player = GameObject.FindWithTag("Player"); 
    player.transform.position = _playerPosition;

    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
}