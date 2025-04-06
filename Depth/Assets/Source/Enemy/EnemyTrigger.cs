using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerView player;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            Debug.Log("ПОДПИСАЛСЯ");
            player.OnKill += enemy.Die;
            player.OnKill += BodyDead;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskCheck.ContainsLayer(playerLayer, other.gameObject.layer))
        {
            if (enemy != null)
            {
                Debug.Log("ОДПИСАЛСЯ");
                player.OnKill -= enemy.Die;
                player.OnKill -= BodyDead;
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("ОДПИСАЛСЯ после дизабле");
    }

    private void OnDestroy()
    {
        player.OnKill -= BodyDead;
        Debug.Log("ОДПИСАЛСЯ после дестроя");
    }

    private void BodyDead()
    {
        player.OnKill -= BodyDead;
        player.OnKill -= enemy.Die;
        Destroy(enemy.gameObject);
    }
}
