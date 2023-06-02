using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChareacterSelectReady : NetworkBehaviour
{
    public static ChareacterSelectReady Instance { get; private set; }

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
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }
}
