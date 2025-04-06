using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public Transform target; 
    private NavMeshAgent _agent; 

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (target != null)
        {
            _agent.SetDestination(target.position); 
        }
    }
}
