using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicMenu : MonoBehaviour
{
    public static MusicMenu Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _audioSource.volume = PlayerPrefs.GetFloat(MusicManager.PLAYER_PREFS_MUSIC_VOLUME, 0.5f);
    }
}
