using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using TMPro;
using UnityEngine.UI;

public class LobbyItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyName;

    private Lobby _lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinByID(_lobby.Id);
        });
    }

    public void SetLobby(Lobby lobby)
    {
        this._lobby = lobby;
        _lobbyName.text = lobby.Name;
    }
}
