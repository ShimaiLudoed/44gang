using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private float stepRate = 0.5f;
    [SerializeField] private AudioClip footstepSounds;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashCooldown = 1f;
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float respawnDelay = 1f;
    
    [SerializeField] private SwitchWorld switchWorld;

    private Animator _animator;
    private Camera _camera;
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    private float _stepTimer = 0f;
    private float _lastDashTime = 0f;
    private bool _canDash = false;

    public event Action OnKill;
    public event Action OnSave;
    public event Action OnFinish;

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

        if (direction.magnitude > 0f)
        {
            _stepTimer += Time.deltaTime;

            if (_stepTimer >= stepRate)
            {
                PlayFootstepSound();
                _stepTimer = 0f;
            }
            //_animator.SetBool("isWalking", true);
        }
        else
        {
            _stepTimer = 0f; 
            //_animator.SetBool("isWalking", false);
        }
    }

    private void PlayFootstepSound()
    {
        footstepAudioSource.clip = footstepSounds;
        footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "level")
        {
            _canDash = false;
        }
        else
        {
            _canDash = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnSave?.Invoke();
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            if (OnFinish != null)
            {
                OnFinish.Invoke();
            }
        }
    }

    public void Kill()
    {
        OnKill?.Invoke();   
    }

    public void Die()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        Vector3 savedPosition = playerManager.LoadPlayerPosition();
        transform.position = savedPosition;
        Debug.Log("Player respawned at: " + savedPosition);
    }

    public void Dash()
    {
        if (_canDash && Time.time >= _lastDashTime + dashCooldown)
        {
            Vector3 dashDirection = transform.forward.normalized; 
            Vector3 dashTargetPosition = transform.position + dashDirection * dashDistance; 
            
            if (Physics.Raycast(transform.position, dashDirection, dashDistance))
            {
                Debug.Log("Dash blocked by a wall or obstacle.");
                return;
            }

            _rb.MovePosition(dashTargetPosition);
            _lastDashTime = Time.time;
        }
    }
}
