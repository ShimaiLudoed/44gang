using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SwitchWorld switchWorld;
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Camera _camera;
    private Vector3 _moveDirection;
    [SerializeField] private float dashDistance = 10f;
    public bool _canDash = false;


    void Start()
    {
        switchWorld.OnChange += ChangeDash;
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    private void ChangeDash()
    {
        _canDash = !_canDash;
    }

    public void Move(Vector3 direction)
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = _camera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        _moveDirection = cameraForward * direction.z + cameraRight * direction.x;
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        _rb.velocity = _moveDirection * speed;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Norm")
        {
            _canDash = false;
        }
        else
        {
            _canDash = true;
        }
    }

    public void Dash()
    {
        if (_canDash == true)
        {
            Vector3 dashDirection = transform.forward;
            transform.position += dashDirection * dashDistance;
        }
    }
}
