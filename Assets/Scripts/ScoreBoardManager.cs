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
    public event EventHandler<OnScoreBoardPlayersCreateArgs> OnScoreBoardPlayersCreated;
    public class OnScoreBoardPlayersCreateArgs : EventArgs
    {
        public Dictionary<ulong, ScoreBoardStruct> ScoreBoardDictionary;
    }

    private Dictionary<ulong, ScoreBoardStruct> _scoreBoardDictionary;
    [SerializeField] string tt;
    private int _scorePerKill = 1;
    private int _testNum; //! for test player num

    private void Awake()
    {
        Instance = this;
        _scoreBoardDictionary = new Dictionary<ulong, ScoreBoardStruct>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
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

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            if (IsServer)
            {
                foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    /* //? for get player name later
                    if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientID, out NetworkClient networkClient))
                    {
                        return;
                    }
                    if (!networkClient.PlayerObject.TryGetComponent<Player>(out Player player))
                    {
                        return;
                    }
                    */
                    //! "gg" is test player name only change later!
                    AddScoreBoardDictionaryClientRpc(clientID, new ScoreBoardStruct(clientID.ToString()));
                }
            }
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            if (!IsOwner) return;
            OnScoreBoardPlayerCreateEventServerRpc();
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void OnScoreBoardPlayerCreateEventServerRpc()
    {
        OnScoreBoardPlayerCreateEventClientRpc();
    }

    [ClientRpc]
    private void OnScoreBoardPlayerCreateEventClientRpc()
    {
        Debug.Log(_scoreBoardDictionary.Count);
        OnScoreBoardPlayersCreated?.Invoke(this, new OnScoreBoardPlayersCreateArgs
        {
            ScoreBoardDictionary = _scoreBoardDictionary,
        });
    }

    [ClientRpc]
    private void AddScoreBoardDictionaryClientRpc(ulong clientID, ScoreBoardStruct scoreBoardStruct)
    {
        _scoreBoardDictionary.Add(clientID, scoreBoardStruct);
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
        // OnScoreBoardKillChanged?.Invoke(this, new OnScoreBoardScoreChangedArgs
        // {
        //     ClientID = clientID
        // });
    }
}
