using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using IngameDebugConsole;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    public event EventHandler OnCreateLobbyStart;
    public event EventHandler OnCreateLobbyFail;
    public event EventHandler OnJoinStart;
    public event EventHandler OnJoinFail;
    public event EventHandler OnQuickJoinFail;
    public event EventHandler<OnLobbiesListChangedArgs> OnLobbiesListChanged;
    public class OnLobbiesListChangedArgs : EventArgs
    {
        public List<Lobby> LobbyList;
    }

    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";

    private Lobby _joinedLobby;
    private float _heartbeatTimer;
    private float _listLobbiesTimer;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }

    private void Update()
    {
        HandleHeartbeat();
        HandlePeriodicListlobbies();
    }

    private void HandlePeriodicListlobbies()
    {
        if (_joinedLobby == null &&
        AuthenticationService.Instance.IsSignedIn &&
        SceneManager.GetActiveScene().name == Loader.Scene.LobbyScene.ToString())
        {
            _listLobbiesTimer -= Time.deltaTime;
            if (_listLobbiesTimer <= 0)
            {
                float listLobbiesTimerMax = 3f;
                _listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }
    }

    private void HandleHeartbeat()
    {
        if (IsLobbyHost())
        {
            _heartbeatTimer -= Time.deltaTime;
            if (_heartbeatTimer <= 0f)
            {
                float heartbeatTimerMax = 15f;
                _heartbeatTimer = heartbeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async Task<Allocation> AllocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(GameMultiplayer.MAX_PLAYER_LIMIT - 1);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }

    }

    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStart?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, GameMultiplayer.MAX_PLAYER_LIMIT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });

            Allocation allocation = await AllocateRelay();

            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>{
                    {KEY_RELAY_JOIN_CODE,new DataObject(DataObject.VisibilityOptions.Member,relayJoinCode)}
                }
            });

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            GameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.LobbyRoomScene);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnCreateLobbyFail?.Invoke(this, EventArgs.Empty);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>{
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
            }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            OnLobbiesListChanged?.Invoke(this, new OnLobbiesListChangedArgs
            {
                LobbyList = queryResponse.Results
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoin()
    {
        OnJoinStart?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            string relayJoinCode = _joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            GameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnQuickJoinFail?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void JoinByID(string lobbyID)
    {
        OnJoinStart?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);

            string relayJoinCode = _joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            GameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFail?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void JoinByCode(string lobbyCode)
    {
        OnJoinStart?.Invoke(this, EventArgs.Empty);
        try
        {
            _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = _joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            GameMultiplayer.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFail?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void DeleteLobby()
    {
        if (_joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                _joinedLobby = null;

            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby()
    {
        if (_joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerID)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerID);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public Lobby GetLobby()
    {
        return _joinedLobby;
    }
}
