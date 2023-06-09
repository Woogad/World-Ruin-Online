using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public class CharacterLobbyRoomUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;
    [SerializeField] private Button _readyBn;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    private void Awake()
    {
        _mainMenuBn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        _readyBn.onClick.AddListener(() =>
        {
            ChareacterSelectReady.Instance.SetPlayerReady();
        });
    }

    private void Start()
    {
        Lobby lobby = LobbyManager.Instance.GetLobby();

        _lobbyNameText.text = "Lobby Name: " + lobby.Name;
        _lobbyCodeText.text = "Code: " + lobby.LobbyCode;
    }
}
