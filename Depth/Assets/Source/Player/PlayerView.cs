using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Camera _camera;
    public Vector3 moveDirection;


    void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = _camera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        moveDirection = cameraForward * direction.z + cameraRight * direction.x;
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        _rb.velocity = moveDirection * speed;
    }
    
}
