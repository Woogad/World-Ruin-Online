using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using IngameDebugConsole;

public class ScoreBoardManager : NetworkBehaviour
{
    public static ScoreBoardManager Instance { get; private set; }

    public event EventHandler<OnScoreBoardChangedArgs> OnDeleteScoreBoardItem;
    public event EventHandler<OnScoreBoardChangedArgs> OnScoreBoardChanged;
    public class OnScoreBoardChangedArgs : EventArgs
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
        DebugLogConsole.AddCommand<ulong>("AddScoreKill", "To add kill with clientID", AddScoreByKillServerRpc);
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
            _scoreBoardDictionary.Add(playerData.ClientID, new ScoreBoardStruct(playerData.PlayerName));
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
        AddScoreByKillServerRpc(e.KillerClientID);
    }

    private void NetworkManagerOnClientDisConnectCallback(ulong clientID)
    {
        DeleteScoreBoardItemServerRpc(clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreServerRpc(int score, ulong ClientID)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(ClientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        player.AddPlayerScoreNetworkVariable(score);
        AddScoreClientRpc(score, ClientID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreByKillServerRpc(ulong killerClientID)
    {
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(killerClientID, out NetworkClient networkClient))
        {
            return;
        }
        if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        player.AddPlayerScoreNetworkVariable(_scorePerKill);
        AddScoreClientRpc(_scorePerKill, killerClientID);
    }

    [ClientRpc]
    private void AddScoreClientRpc(int score, ulong ClientID)
    {
        var newScore = _scoreBoardDictionary[ClientID];
        newScore.AddScoreKill(score);
        _scoreBoardDictionary[ClientID] = newScore;

        OnScoreBoardChanged?.Invoke(this, new OnScoreBoardChangedArgs
        {
            ClientID = ClientID,
            Value = _scoreBoardDictionary[ClientID].KillScore
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
        OnDeleteScoreBoardItem?.Invoke(this, new OnScoreBoardChangedArgs
        {
            ClientID = clientID
        });
    }

    public Dictionary<ulong, ScoreBoardStruct> GetScoreBoardDictionary()
    {
        return this._scoreBoardDictionary;
    }
}
