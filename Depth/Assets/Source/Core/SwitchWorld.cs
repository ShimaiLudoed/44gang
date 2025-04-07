using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchWorld : MonoBehaviour
{
    public event Action OnChange;
    private Vector3 _playerPosition;
    private Quaternion _playerRotation;

    private void Start()
    {
        EnemyManager.ClearData(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeWorld();
        }
    }

    private void ChangeWorld()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerPosition = player.transform.position;
            _playerRotation = player.transform.rotation;
        }
        
        SaveEnemiesData();

        if (SceneManager.GetActiveScene().name == "level 1")
        {
            SceneManager.LoadScene("level");
        }
        else
        {
            SceneManager.LoadScene("level 1");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SaveEnemiesData()
    {
        EnemyManager.ClearData(); 
        foreach (var enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            EnemyManager.SetEnemyData(enemy.enemyId, enemy.transform.position, enemy.transform.rotation, false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = _playerPosition;
            player.transform.rotation = _playerRotation;
        }


        RestoreEnemiesData();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void RestoreEnemiesData()
    {
        foreach (var enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            EnemyData enemyData = EnemyManager.GetEnemyData(enemy.enemyId);
            if (enemyData != null && !enemyData.isDead)
            {
                enemy.transform.position = enemyData.position;
                enemy.transform.rotation = enemyData.rotation;
            }
            else
            {
                Destroy(enemy.gameObject); 
            }
        }
    }
}
