using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchWorld : MonoBehaviour
{
    public event Action OnChange;
    private Vector3 _playerPosition;
    private Quaternion _playerRotation;

    [SerializeField] private float ChangeCooldown = 3f;
    private float _lastChangeTime = 0f;
    
    private void Start()
    { 
        EnemyManager.ClearData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= _lastChangeTime + ChangeCooldown)
            {
                ChangeWorld();
                _lastChangeTime = Time.time; 
            }
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

        if (SceneManager.GetActiveScene().name == "level")
        {
            SceneManager.LoadScene("Curse");
        }
        else
        {
            SceneManager.LoadScene("level");
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
