using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {

        [Header("Persistence")]
        [SerializeField] private string keypadID = "door01"; // Уникальный ID

        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); // orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); // red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); // greenish
        
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        [Header("UI Text Settings")]
        [SerializeField] private bool showUITextOnEnter = true;
        [SerializeField] private TMP_Text uiText;
        [TextArea(2, 5)] [SerializeField] private string messageToShow = "Введите код, используя цифры на клавиатуре...";
        [SerializeField] private float typeSpeed = 0.05f;
        [SerializeField] private float waitBeforeFade = 2f;
        
        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;
        private bool playerInTrigger = false;
        private Coroutine textRoutine;

        private void Awake()
        {

            if (KeypadManager.Instance != null && KeypadManager.Instance.IsAccessGranted(keypadID))
            {
                accessWasGranted = true;
                AccessGranted();
            }

            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            if (uiText != null) uiText.text = "";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = true;

                if (showUITextOnEnter && uiText != null)
                {
                    if (textRoutine != null) StopCoroutine(textRoutine);
                    textRoutine = StartCoroutine(ShowUITextRoutine());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = false;
                ClearInput();
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

                if (showUITextOnEnter && uiText != null)
                {
                    if (textRoutine != null) StopCoroutine(textRoutine);
                    uiText.text = "";
                }
            }
        }

        private void Update()
        {
            if (!playerInTrigger || displayingResult || accessWasGranted) return;

            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)) ||
                    Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i)))
                {
                    AddInput(i.ToString());
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CheckCombo();
            }
        }

        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (currentInput != null && currentInput.Length == 9) return;

            currentInput += input;
            keypadDisplayText.text = currentInput;
        }

        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input for some reason..");
            }
        }

        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted)
                AccessGranted();
            else
                AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;

            if (granted) yield break;

            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        private void AccessGranted()
        {
            KeypadManager.Instance?.SetAccessGranted(keypadID);
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
        }

        private IEnumerator ShowUITextRoutine()
        {
            uiText.text = "";
            foreach (char c in messageToShow)
            {
                uiText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }

            yield return new WaitForSeconds(waitBeforeFade);

            for (int i = uiText.text.Length; i > 0; i--)
            {
                uiText.text = uiText.text.Substring(0, i - 1);
                yield return new WaitForSeconds(typeSpeed);
            }
        }
    }
}
