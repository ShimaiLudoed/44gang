using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager 
{
    private static Dictionary<int, EnemyData> _enemyStates = new Dictionary<int, EnemyData>();

    public static void SetEnemyData(int enemyId, Vector3 position, Quaternion rotation, bool isDead)
    {
        _enemyStates[enemyId] = new EnemyData { position = position, rotation = rotation, isDead = isDead };
    }

    public static EnemyData GetEnemyData(int enemyId)
    {
        if (_enemyStates.TryGetValue(enemyId, out EnemyData enemyData))
        {
            return enemyData;
        }
        return null;
    }

    public static void ClearData()
    {
        _enemyStates.Clear();
    }
}
