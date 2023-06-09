using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyMessagesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messagesText;
    [SerializeField] private Button _closeBn;

    private void Awake()
    {
        _closeBn.onClick.AddListener(Hide);
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnFailToJoinGame += GameMultiplayerOnFailToJoinGame;
        LobbyManager.Instance.OnCreateLobbyStart += LobbyManagerOnCreateLobbyStart;
        LobbyManager.Instance.OnCreateLobbyFail += LobbyManagerOnCreateLobbyFail;
        LobbyManager.Instance.OnJoinStart += LobbyManagerOnJoinStart;
        LobbyManager.Instance.OnJoinFail += LobbyManagerOnJoinFail;
        LobbyManager.Instance.OnQuickJoinFail += LobbyManagerOnQuickJoinFail;
        Hide();
    }

    private void LobbyManagerOnQuickJoinFail(object sender, EventArgs e)
    {
        ShowMessages("Could not find a Lobby to Quick Join!");
    }

    private void LobbyManagerOnJoinFail(object sender, EventArgs e)
    {
        ShowMessages("Fail to Join Lobby!");
    }

    private void LobbyManagerOnJoinStart(object sender, EventArgs e)
    {
        ShowMessages("Joining Lobby...");
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailToJoinGame -= GameMultiplayerOnFailToJoinGame;
        LobbyManager.Instance.OnCreateLobbyStart -= LobbyManagerOnCreateLobbyStart;
        LobbyManager.Instance.OnCreateLobbyFail -= LobbyManagerOnCreateLobbyFail;
        LobbyManager.Instance.OnJoinStart -= LobbyManagerOnJoinStart;
        LobbyManager.Instance.OnJoinFail -= LobbyManagerOnJoinFail;
        LobbyManager.Instance.OnQuickJoinFail -= LobbyManagerOnQuickJoinFail;
    }

    private void LobbyManagerOnCreateLobbyFail(object sender, EventArgs e)
    {
        ShowMessages("Fail to Create Lobby!");
    }

    private void LobbyManagerOnCreateLobbyStart(object sender, EventArgs e)
    {
        ShowMessages("Creating Lobby...");
    }

    private void GameMultiplayerOnFailToJoinGame(object sender, EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessages("Fail to connect!");
        }
        else
        {
            ShowMessages(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessages(string messages)
    {
        Show();
        _messagesText.text = messages;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
