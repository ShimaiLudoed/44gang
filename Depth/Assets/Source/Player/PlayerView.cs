using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private int maxHealth = 100; // Максимальное здоровье
    public int currentHealth; // Текущее здоровье

    [SerializeField] private float healthRecoveryRate = 1f; // Здоровье за секунду (в обычном мире)
    [SerializeField] private float healthLossRate = 1f; // Потеря здоровья за секунду (в другом мире)

    private Coroutine healthCoroutine; // Ссылка на корутину
    public bool isInNormalWorld; // Флаг для проверки, в каком мире игрок
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
        currentHealth = maxHealth; // Устанавливаем текущее здоровье на максимум
        ChangeWorld();
        
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


    public void ChangeWorld()
        {

            // Запускаем или останавливаем корутину в зависимости от мира
            if (isInNormalWorld)
            {
                StartHealthRecovery();
            }
            else
            {
                StartHealthLoss();
            }
        }

    private void StartHealthRecovery()
        {
            if (healthCoroutine != null) StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(RecoverHealth());
        }
    private void StartHealthLoss()
        {
            if (healthCoroutine != null) StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(LoseHealth());
        }
    private IEnumerator LoseHealth()
        {
            while (currentHealth > 0)
            {
                currentHealth -= Mathf.FloorToInt(healthLossRate);
                currentHealth = Mathf.Max(currentHealth, 0); // Убедитесь, что здоровье не менее 0
                yield return new WaitForSeconds(2f); // Подождите 1 секунду перед потерей
                UpdateHealthSprite(); // Обновите спрайт здоровья
            }

            // Здесь можно вызывать метод умирания игрока
            Die(); 
        }

    private IEnumerator RecoverHealth()
        {
            while (currentHealth < maxHealth)
            {
                currentHealth += Mathf.FloorToInt(healthRecoveryRate);
                currentHealth = Mathf.Min(currentHealth, maxHealth); // Ограничиваем здоровье максимум
                yield return new WaitForSeconds(1f); // Подождите 1 секунду перед восстановлением
                UpdateHealthSprite(); // Обновите спрайт здоровья
            }
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


    private void UpdateHealthSprite()
    {
        float healthPercentage = (float)currentHealth / maxHealth; // Изменяем процент здоровья

        // Определяем индекс спрайта исходя из процента здоровья
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (healthSprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, healthSprites.Length - 1);
        healthImage.sprite = healthSprites[spriteIndex];

    }


}
