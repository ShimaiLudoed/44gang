using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioClip scene1Music;
    public AudioClip scene2Music;

    private AudioSource audioSource;
    private static MusicManager instance;

    // Словарь для хранения прогресса воспроизведения
    private Dictionary<string, float> musicProgress = new Dictionary<string, float>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Сохраняем время предыдущей сцены
        if (audioSource.clip != null)
        {
            musicProgress[SceneManager.GetActiveScene().name] = audioSource.time;
        }

        // Назначаем нужный трек
        switch (sceneName)
        {
            case "level":
                SetMusic(scene1Music, sceneName);
                break;
            case "Curse":
                SetMusic(scene2Music, sceneName);
                break;
        }
    }

    void SetMusic(AudioClip clip, string sceneName)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;

        // Если мы уже были на этой сцене — продолжаем с того же места
        if (musicProgress.ContainsKey(sceneName))
        {
            audioSource.time = musicProgress[sceneName];
        }
        else
        {
            audioSource.time = 0f;
        }

        audioSource.Play();
    }
}
