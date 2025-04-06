using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    public int enemyId;
    [SerializeField] private bool CanBeDead;
    private void Start()
    {
        EnemyData enemyData = EnemyManager.GetEnemyData(enemyId);
        if (enemyData != null && enemyData.isDead)
        {
            Destroy(gameObject);
        }
        else
        {
            if (enemyData != null)
            {
                transform.position = enemyData.position;
                transform.rotation = enemyData.rotation;
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("Тело");
    }

    public void Die()
    {
        if (CanBeDead)
        {
            EnemyManager.SetEnemyData(enemyId, transform.position, transform.rotation, true);
        }
    }
}
