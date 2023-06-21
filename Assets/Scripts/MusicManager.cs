using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public const float DEFAULT_VOLUME = 0.5f;

    private AudioSource _audioSource;
    private float _volume;


    private void Awake()
    {
        Instance = this;
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, DEFAULT_VOLUME);
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _audioSource.volume = _volume;
        if (SceneManager.GetActiveScene().name == Loader.Scene.GameScene.ToString())
        {
            float fadeTime = 3f;
            StartCoroutine(StartFadeMusic(fadeTime));
        }
    }

    private IEnumerator StartFadeMusic(float duration)
    {
        float FadeTimer = 0;
        float start = 0;
        while (FadeTimer < duration)
        {
            FadeTimer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, _volume, FadeTimer / duration);
            yield return null;
        }
        yield break;
    }

    public void UpVolume()
    {
        _volume += 0.1f;
        _volume = Mathf.Round(_volume * 100f) / 100f; //? For fix float error added

        if (_volume > 1f)
        {
            _volume = 0f;
        }

        _audioSource.volume = _volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }
    public void LowVolume()
    {
        _volume -= 0.1f;
        _volume = Mathf.Round(_volume * 100f) / 100f; //? For fix float error added

        if (_volume < 0f)
        {
            _volume = 1f;
        }

        _audioSource.volume = _volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return this._volume;
    }

}
