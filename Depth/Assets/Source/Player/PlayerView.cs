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
    private float stepTimer = 0f;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float respawnDelay = 1f;
    [SerializeField] private SwitchWorld switchWorld;
    [SerializeField] private float speed;
    private Rigidbody _rb;
    private Camera _camera;
    private Vector3 _moveDirection;
    [SerializeField] private float dashDistance = 10f;
    public bool _canDash = false;
    public event Action OnKill;

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
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepRate)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; 
        }
    }
    private void PlayFootstepSound()
    {
        footstepAudioSource.clip = footstepSounds;
        footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "level1")
        {
            _canDash = false;
        }
        else
        {
            _canDash = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateCheckpoint();
        }
        
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            Die();
        }
    }
    
    private void ActivateCheckpoint()
    {
        playerManager.SavePlayerPosition(transform.position);
    }

    public void Kill()
    {
     OnKill?.Invoke();   
    }
    
    private void Die()
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
        if (_canDash == true)
        {
            Vector3 dashDirection = transform.forward;
            transform.position += dashDirection * dashDistance;
        }
    }
}
