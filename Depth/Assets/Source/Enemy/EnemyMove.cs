using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private PlayerView player;
    [SerializeField] private Transform target; 
    [SerializeField] private float detectionRadius;  
    [SerializeField] private List<Transform> waypoints;  
    private NavMeshAgent _agent;
    private int _currentWaypointIndex = 0;  
    [SerializeField]private bool _isPatrolling;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (waypoints.Count > 0)
        {
            _agent.SetDestination(waypoints[_currentWaypointIndex].position);
        }
    }
    
    private void Update()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= detectionRadius)
        {
            _isPatrolling = false;  
            _agent.SetDestination(target.position);  
        }
        else
        {
            if (_isPatrolling==true)
            {
                Patrol();
            }
        }
    }

    private void Patrol()
    {
        if (waypoints.Count == 0) return; 
        
        Debug.Log("бегу");
        
        if (Vector3.Distance(transform.position, waypoints[_currentWaypointIndex].position) < 1f)
        {
            Debug.Log("дошёл до цели");
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
        }
        Debug.Log("пошёл к другой");
        _agent.SetDestination(waypoints[_currentWaypointIndex].position);
    }
}
