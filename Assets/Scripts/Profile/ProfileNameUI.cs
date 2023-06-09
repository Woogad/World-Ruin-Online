using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileNameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;

    private void Start()
    {
        _playerNameInput.text = PlayerPrefs.GetString(GameMultiplayer.PLAYER_PREFS_PLAYER_NAME, "Player");
        _playerNameInput.onValueChanged.AddListener((string newText) =>
        {
            PlayerPrefs.SetString(GameMultiplayer.PLAYER_PREFS_PLAYER_NAME, newText);
        });
    }

}
