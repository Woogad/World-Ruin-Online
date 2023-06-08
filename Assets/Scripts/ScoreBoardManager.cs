using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using IngameDebugConsole;

public class ScoreBoardManager : NetworkBehaviour
{
    public static ScoreBoardManager Instance { get; private set; }

    public event EventHandler<OnScoreBoardScoreChangedArgs> OnDeleteScoreBoardItem;
    public event EventHandler<OnScoreBoardScoreChangedArgs> OnScoreBoardKillChanged;
    public class OnScoreBoardScoreChangedArgs : EventArgs
    {
        public ulong ClientID;
        public int Value = default;
    }
    private Dictionary<ulong, ScoreBoardStruct> _scoreBoardDictionary = new Dictionary<ulong, ScoreBoardStruct>();
    private int _scorePerKill = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisConnectCallback;
        DebugLogConsole.AddCommand<ulong>("AddScoreKill", "To add kill with clientID", AddKillScoreServerRpc);
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnDead += PlayerOnDead;
        }
        else
        {
            Player.OnAnyPlayerSpawned += PlayerOnAnyPlayerSpawned;
        }

        foreach (PlayerData playerData in GameMultiplayer.Instance.GetPlayerDataNetworkList())
        {
            _scoreBoardDictionary.Add(playerData.ClientID, new ScoreBoardStruct(playerData.ClientID.ToString()));
        }
    }

    private void PlayerOnAnyPlayerSpawned(object sender, EventArgs e)
    {
        if (Player.LocalInstance != null)
        {

            Player.LocalInstance.OnDead -= PlayerOnDead;
            Player.LocalInstance.OnDead += PlayerOnDead;
        }
    }

    private void PlayerOnDead(object sender, Player.OnDeadArgs e)
    {
        AddKillScoreServerRpc(e.KillerClientID);
    }

    private void NetworkManagerOnClientDisConnectCallback(ulong clientID)
    {
        DeleteScoreBoardItemServerRpc(clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddKillScoreServerRpc(ulong shootOwnerClientID)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(shootOwnerClientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        player.AddKillScoreNetworkVariable(_scorePerKill);
        AddKillScoreClientRpc(shootOwnerClientID);
    }

    [ClientRpc]
    private void AddKillScoreClientRpc(ulong shootOwnerClientID)
    {
        var newScore = _scoreBoardDictionary[shootOwnerClientID];
        newScore.AddScoreKill(_scorePerKill);
        _scoreBoardDictionary[shootOwnerClientID] = newScore;

        OnScoreBoardKillChanged?.Invoke(this, new OnScoreBoardScoreChangedArgs
        {
            ClientID = shootOwnerClientID,
            Value = _scoreBoardDictionary[shootOwnerClientID].KillScore
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeleteScoreBoardItemServerRpc(ulong clientID)
    {
        DeleteScoreBoardItemClientRpc(clientID);
    }

    [ClientRpc]
    private void DeleteScoreBoardItemClientRpc(ulong clientID)
    {
        _scoreBoardDictionary.Remove(clientID);
        OnDeleteScoreBoardItem?.Invoke(this, new OnScoreBoardScoreChangedArgs
        {
            ClientID = clientID
        });
    }

    public Dictionary<ulong, ScoreBoardStruct> GetScoreBoardDictionary()
    {
        return this._scoreBoardDictionary;
    }
}
