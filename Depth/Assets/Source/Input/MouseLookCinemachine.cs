using UnityEngine;
using UnityEngine.Serialization;

namespace Support.StarterAssets.ThirdPersonController.Scripts
{
    public class MouseLookCinemachine : MonoBehaviour
    {
        [SerializeField] private Transform firstCamera; 
        [SerializeField] private float mouseSensitivity = 100f; 
        private float xRotationFPS = 0f; 
        [SerializeField] private Transform player;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false; 
        }

        private void Update()
        {
            HandleMouseLook(); // Обработка стыков камеры
        }

        private void HandleMouseLook()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
  
                xRotationFPS -= mouseY; 
                xRotationFPS = Mathf.Clamp(xRotationFPS, -90f, 90f); 
                firstCamera.localRotation = Quaternion.Euler(xRotationFPS, 0f, 0f); 
                
                player.transform.Rotate(Vector3.up * mouseX);
            }
        }
    }

