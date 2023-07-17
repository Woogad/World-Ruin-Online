using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button _closeBn;
    [SerializeField] private Button _createPublicBn;
    [SerializeField] private Button _createPrivateBn;
    [SerializeField] private TMP_InputField _lobbyNameInput;

    private const int LOBBY_NAME_CHARACTER_LIMIT = 16;
    private const string DEFAULT_LOBBY_NAME = "Lobby Name";

    private void Awake()
    {
        _lobbyNameInput.characterLimit = LOBBY_NAME_CHARACTER_LIMIT;

        _closeBn.onClick.AddListener(() =>
        {
            Hide();
        });
        _createPublicBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInput.text, false);
        });
        _createPrivateBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.CreateLobby(_lobbyNameInput.text, true);
        });
    }

    private void Start()
    {
        _lobbyNameInput.onEndEdit.AddListener((string text) =>
        {
            if (string.IsNullOrEmpty(_lobbyNameInput.text))
            {
                _lobbyNameInput.text = DEFAULT_LOBBY_NAME;
            }
            else if (string.IsNullOrWhiteSpace(_lobbyNameInput.text))
            {
                _lobbyNameInput.text = DEFAULT_LOBBY_NAME;
            }
        });
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
