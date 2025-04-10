using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Объект, который будем выключать/включать
    [SerializeField] private AudioClip activationSound; // Аудиоклип для воспроизведения при активации
    private AudioSource audioSource; // Аудиоисточник для воспроизведения звука

    private bool isInCooldown = false;

    private void Start()
    {
        // Добавляем компонент AudioSource, если его нет, и настраиваем его
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Отключаем воспроизведение при запуске
    }

    private void OnTriggerStay(Collider other)
    {
        // Проверяем, что в зоне находится игрок и нажата клавиша F
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            // Если не в перерыве, запускаем процедуру
            if (targetObject != null && !isInCooldown)
            {
                StartCoroutine(ToggleObjectCoroutine());
            }
        }
    }

    private IEnumerator ToggleObjectCoroutine()
    {
        isInCooldown = true;

        // Воспроизводим звук, если он установлен
        if (activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }

        // Отключаем объект
        targetObject.SetActive(false);
        // Ждём 10 секунд
        yield return new WaitForSeconds(10f);
        // Включаем объект обратно
        targetObject.SetActive(true);

        isInCooldown = false;
    }
}
