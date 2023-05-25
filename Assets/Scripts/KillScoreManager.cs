using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using IngameDebugConsole;

public class KillScoreManager : NetworkBehaviour
{
    public static KillScoreManager Instance { get; private set; }

    public event EventHandler<OnScoreBoardPlayersChangedArgs> OnScoreBoardPlayersChanged;
    public class OnScoreBoardPlayersChangedArgs : EventArgs
    {
        public Dictionary<ulong, ScoreBoardStruct> ScoreBoardDictionary;
        public bool IsInit;
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
        DebugLogConsole.AddCommand<ulong>("AddScoreKill", "To add kill with clientID", AddKillScoreServerRpc);
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
                    AddScoreBoardDictionaryClientRpc(clientID, new ScoreBoardStruct("gg"));
                }
            }
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            if (!IsOwner) return;
            OnScoreBoardPlayerChangedEventServerRpc(true);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void OnScoreBoardPlayerChangedEventServerRpc(bool isInit)
    {
        OnScoreBoardPlayerChangedEventClientRpc(isInit);
    }

    [ClientRpc]
    private void OnScoreBoardPlayerChangedEventClientRpc(bool isInit)
    {

        OnScoreBoardPlayersChanged?.Invoke(this, new OnScoreBoardPlayersChangedArgs
        {
            ScoreBoardDictionary = _scoreBoardDictionary,
            IsInit = isInit
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
        player.AddKillScoreNetworkVariableServerRpc(_scorePerKill);
        AddKillScoreClientRpc(shootOwnerClientID);
    }

    [ClientRpc]
    private void AddKillScoreClientRpc(ulong shootOwnerClientID)
    {
        var newScore = _scoreBoardDictionary[shootOwnerClientID];
        newScore.AddScoreKill(_scorePerKill);
        _scoreBoardDictionary[shootOwnerClientID] = newScore;

        OnScoreBoardPlayerChangedEventServerRpc(false);
    }

}
