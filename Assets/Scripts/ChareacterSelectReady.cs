using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChareacterSelectReady : NetworkBehaviour
{
    public static ChareacterSelectReady Instance { get; private set; }

    public event EventHandler OnReadyChanged;

    private event Action _readyClientRpcName;

    AudioSource audioSource;
    private NetworkList<PlayerReady> _playerReadyList;



    private void Awake()
    {
        Instance = this;
        _playerReadyList = new NetworkList<PlayerReady>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(SoundManager.PLAYER_PREFS_SOUND_EFFECT_VOLUME, 0.8f);

        _readyClientRpcName = this.SetPlayerReadyClientRpc;

        if (!IsServer) return;
        _playerReadyList.OnListChanged += PlayerReadyListOnlistChanged;
        _playerReadyList.Add(new PlayerReady
        {
            ClientID = NetworkManager.ServerClientId,
            IsReady = false
        });
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManagerOnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnDisConnectCallback;
    }

    private void PlayerReadyListOnlistChanged(NetworkListEvent<PlayerReady> changeEvent)
    {
        SetPlayerReadyClientRpc();

        Invoke(_readyClientRpcName.Method.Name, 0.1f);
    }

    private void NetworkManagerOnDisConnectCallback(ulong ClientID)
    {
        foreach (var player in _playerReadyList)
        {
            if (player.ClientID == ClientID)
            {
                int disconnectPlayerIndex = _playerReadyList.IndexOf(player);
                _playerReadyList.RemoveAt(disconnectPlayerIndex);
                break;
            }
        }
    }

    private void NetworkManagerOnClientConnectedCallback(ulong ClientID)
    {
        _playerReadyList.Add(new PlayerReady
        {
            ClientID = ClientID,
            IsReady = false
        });
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    public void SetPlayerUnReady()
    {
        SetPlayerUnReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        foreach (var player in _playerReadyList)
        {
            if (player.ClientID == serverRpcParams.Receive.SenderClientId)
            {
                PlayerReady playerReady = new PlayerReady
                {
                    ClientID = serverRpcParams.Receive.SenderClientId,
                    IsReady = true
                };
                int readyPlayerIndex = _playerReadyList.IndexOf(player);
                _playerReadyList[readyPlayerIndex] = playerReady;
                break;
            }
        }
        bool allClientReady = true;

        foreach (var player in _playerReadyList)
        {
            if (!player.IsReady)
            {
                allClientReady = false;
                break;
            }
        }
        if (allClientReady)
        {
            PlayStartSoundClientRpc();
            StartCoroutine(StartGameCountdown(2f));
        }
    }

    private IEnumerator StartGameCountdown(float duration)
    {
        float startTimer = 0;
        while (startTimer < duration)
        {
            startTimer += Time.deltaTime;
            yield return null;
        }
        LobbyManager.Instance.DeleteLobby();
        Loader.LoadNetwork(Loader.Scene.GameScene);
        yield break;
    }

    [ClientRpc]
    private void PlayStartSoundClientRpc()
    {
        audioSource.Play();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerUnReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        foreach (var player in _playerReadyList)
        {
            if (player.ClientID == serverRpcParams.Receive.SenderClientId)
            {
                PlayerReady playerReady = new PlayerReady
                {
                    ClientID = serverRpcParams.Receive.SenderClientId,
                    IsReady = false
                };
                int readyPlayerIndex = _playerReadyList.IndexOf(player);
                _playerReadyList[readyPlayerIndex] = playerReady;
                break;
            }
        }
    }


    [ClientRpc]
    private void SetPlayerReadyClientRpc()
    {
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientID)
    {
        foreach (var player in _playerReadyList)
        {
            if (player.ClientID == clientID)
            {
                return player.IsReady;
            }
        }
        return false;
    }

}
