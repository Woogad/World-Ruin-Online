using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button _mainMenuBn;
    [SerializeField] Button _createLobbyBn;
    [SerializeField] Button _quickJoinBn;
    [SerializeField] LobbyCreateUI _lobbyCreateUI;
    [SerializeField] Button _joinCode;
    [SerializeField] TMP_InputField _lobbyCodeInput;
    [SerializeField] TMP_InputField _playerNameInput;

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        _createLobbyBn.onClick.AddListener(() =>
        {
            _lobbyCreateUI.Show();
        });
        _quickJoinBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.QuickJoin();
        });
        _joinCode.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinByCode(_lobbyCodeInput.text);
        });
    }

    private void Start()
    {
        _playerNameInput.text = GameMultiplayer.Instance.GetPlayerName();
        _playerNameInput.onValueChanged.AddListener((string newText) =>
        {
            GameMultiplayer.Instance.SetPlayerName(newText);
        });
    }
}
