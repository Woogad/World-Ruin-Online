using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using System;

public class LobbyRoomUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuBn;
    [SerializeField] private Button _readyBn;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    [SerializeField] private Button _copyBn;
    [SerializeField] private Button _unReadyBn;

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
            PlayerSelectReady.Instance.SetPlayerReady();
        });
        _copyBn.onClick.AddListener(CopyLobbyCode);
        _unReadyBn.onClick.AddListener(() =>
        {
            PlayerSelectReady.Instance.SetPlayerUnReady();
        });
    }

    private void Start()
    {
        Lobby lobby = LobbyManager.Instance.GetLobby();

        _lobbyNameText.text = lobby.Name;
        _lobbyCodeText.text = "Code: " + lobby.LobbyCode;
        PlayerSelectReady.Instance.OnAllPlayerReady += PlayerSelectReadyOnAllPlayerReady;
    }

    private void PlayerSelectReadyOnAllPlayerReady(object sender, EventArgs e)
    {
        _unReadyBn.interactable = false;
    }

    private void CopyLobbyCode()
    {
        Lobby lobby = LobbyManager.Instance.GetLobby();
        TextEditor textEditor = new TextEditor();
        textEditor.text = lobby.LobbyCode;
        textEditor.SelectAll();
        textEditor.Copy();
    }
}
