using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;

    private void Awake()
    {
        _playerNameInput.characterLimit = GameMultiplayer.NAME_CHARACTER_LIMIT;

        _playerNameInput.contentType = TMP_InputField.ContentType.Alphanumeric;
    }

    private void Start()
    {
        _playerNameInput.text = PlayerPrefs.GetString(GameMultiplayer.PLAYER_PREFS_PLAYER_NAME, GameMultiplayer.DEFAULT_NAME);
        _playerNameInput.onValueChanged.AddListener((string newText) =>
        {
            PlayerPrefs.SetString(GameMultiplayer.PLAYER_PREFS_PLAYER_NAME, newText);
        });
        _playerNameInput.onEndEdit.AddListener((string text) =>
        {
            if (string.IsNullOrEmpty(_playerNameInput.text))
            {
                _playerNameInput.text = GameMultiplayer.DEFAULT_NAME;
                PlayerPrefs.SetString(GameMultiplayer.PLAYER_PREFS_PLAYER_NAME, GameMultiplayer.DEFAULT_NAME);
            }
        });
    }

}
