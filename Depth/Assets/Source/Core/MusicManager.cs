using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip abyssClip;
    [SerializeField] private AudioClip normalClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            GetComponent<AudioSource>().Play(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "level")
        {
            _audioSource.clip = normalClip;
        }
        else
        {
            _audioSource.clip = abyssClip;
        }
    }
}
