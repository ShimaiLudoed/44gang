using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;

    private const string VolumePrefKey = "MasterVolume";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f); // Значение от 0.0001 до 1
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("MasterVolume", volume);

        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }
}