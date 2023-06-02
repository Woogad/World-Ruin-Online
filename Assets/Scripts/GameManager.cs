using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    private enum State
    {
        WaitingToStart,
        CountdonwToStart,
        GamePlaying,
        GameOver
    }

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;

    [SerializeField] private Transform _playerPrefab;

    private NetworkVariable<State> _state = new NetworkVariable<State>();
    private NetworkVariable<float> _countdownToStartTimer = new NetworkVariable<float>(1f);
    private NetworkVariable<float> _gamePlayingTimer = new NetworkVariable<float>(0f);
    private float _gamePlayingTimerMax = 200;
    private bool _isLocalPlayerReady;
    private Dictionary<ulong, bool> _playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        _state.Value = State.WaitingToStart;
        _playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        _state.OnValueChanged += StateOnValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManagerOnLoadEventCompleted;
        }
    }

    private void NetworkManagerOnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(_playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
        }
    }

    private void StateOnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_state.Value == State.WaitingToStart)
        {
            _isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
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
                allClientReady = false;
                break;
            }
        }
        Debug.Log("All Player Ready: " + allClientReady);
        if (allClientReady)
        {
            _state.Value = State.CountdonwToStart;
        }
    }

    private void Update()
    {
        if (!IsServer) return;
        switch (_state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdonwToStart:
                _countdownToStartTimer.Value -= Time.deltaTime;
                if (_countdownToStartTimer.Value < 0)
                {
                    _state.Value = State.GamePlaying;
                    _gamePlayingTimer.Value = _gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (_gamePlayingTimer.Value < 0)
                {
                    _state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return this._state.Value == State.GamePlaying;
    }

    public bool IsGameWaitingPlayer()
    {
        return this._state.Value == State.WaitingToStart;
    }

    public bool IsCountdownToStartActive()
    {
        return this._state.Value == State.CountdonwToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return this._countdownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return this._state.Value == State.GameOver;
    }

    public bool IsLocalPlayerReady()
    {
        return this._isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer.Value / _gamePlayingTimerMax);
    }
}
