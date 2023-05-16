using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private AudioSource _audioSource;
    private float _volume = 0.3f;

    private void Awake()
    {
        Instance = this;
        _volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.5f);
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _audioSource.volume = _volume;
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
