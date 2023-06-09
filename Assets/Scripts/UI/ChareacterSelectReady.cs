using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChareacterSelectReady : NetworkBehaviour
{
    public static ChareacterSelectReady Instance { get; private set; }

    public event EventHandler OnReadyChanged;

    private Dictionary<ulong, bool> _playerReadyDictionary;
    private void Awake()
    {
        Instance = this;
        _playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientReady = true;

        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_playerReadyDictionary.ContainsKey(clientID) || !_playerReadyDictionary[clientID])
            {
                //* This Player not ready!
                allClientReady = false;
                break;
            }
        }
        if (allClientReady)
        {
            LobbyManager.Instance.DeleteLobby();
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientID)
    {
        _playerReadyDictionary[clientID] = true;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientID)
    {
        return _playerReadyDictionary.ContainsKey(clientID) && _playerReadyDictionary[clientID];
    }
}
