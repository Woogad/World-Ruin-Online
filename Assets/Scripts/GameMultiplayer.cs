using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    public event EventHandler OnTryToJoinGame;
    public event EventHandler OnFailToJoinGame;
    public event EventHandler OnDataNetworkListChanged;

    [SerializeField] private GunObjectListSO _gunObjectListSO;
    [SerializeField] private GoldCoinSO _goldCoinSO;
    [SerializeField] private PlayerPrefabListSO _playerPrefabListSO;

    private NetworkList<PlayerData> _playerDataNetworkList;
    private string _playerName;

    public static bool IsPlayMultiplayer;
    public const int MAX_PLAYER_LIMIT = 4;
    public const string PLAYER_PREFS_PLAYER_NAME = "PlayerName";
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME, "Player");
        _playerDataNetworkList = new NetworkList<PlayerData>();
        _playerDataNetworkList.OnListChanged += PlayerDataNetworkListOnListChanged;
    }

    private void Start()
    {
        if (!IsPlayMultiplayer)
        {
            StartHost();
        }
    }

    private void PlayerDataNetworkListOnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public string GetPlayerName()
    {
        return this._playerName;
    }

    public void SetPlayerName(string playerName)
    {
        this._playerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME, _playerName);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManagerConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManagerOnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientID)
    {
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = _playerDataNetworkList[i];
            if (playerData.ClientID == clientID)
            {
                _playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    public void StartClient()
    {
        OnTryToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManagerOnClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManagerOnClientConnectedCallback(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            SetPlayerDataNetworkListServerRpc(
                PlayerPrefs.GetInt(PlayerPrefabManager.PLAYER_PREFS_PLAYER_PREFAB_INDEX, 0),
                PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME, "Player"),
                AuthenticationService.Instance.PlayerId
            );
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerDataNetworkListServerRpc(int index, string playerName, string playerID, ServerRpcParams serverRpcParams = default)
    {
        _playerDataNetworkList.Add(new PlayerData
        {
            ClientID = serverRpcParams.Receive.SenderClientId,
            PlayerPrefabIndex = index,
            PlayerName = playerName,
            PlayerID = playerID,
        });
    }

    public NetworkList<PlayerData> GetPlayerDataNetworkList()
    {
        return this._playerDataNetworkList;
    }

    public PlayerData GetPlayerDataFormPlayerIndex(int playerIndex)
    {
        return _playerDataNetworkList[playerIndex];
    }

    public GameObject GetPlayerPrefabSOFormIndex(int playerPrefabListSOIndex)
    {
        return this._playerPrefabListSO.PlayerPrefabSOList[playerPrefabListSOIndex];
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong obj)
    {
        OnFailToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    private void NetworkManagerConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.LobbyRoomScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_LIMIT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Lobby is full";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }

    public void SpawnGunObject(GunObjectSO gunObjectSO, IGunObjectParent gunObjectParent)
    {
        SpawnGunObjectServerRpc(GetGunObjectSOIndex(gunObjectSO), gunObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnGunObjectServerRpc(int gunObjectSOIndex, NetworkObjectReference gunObjectParentNetworkObjectRef)
    {
        GunObjectSO gunObjectSO = GetGunObjectSOFromIndex(gunObjectSOIndex);

        Transform gunObjectTransform = Instantiate(gunObjectSO.Prefab);

        NetworkObject gunNetworkObject = gunObjectTransform.GetComponent<NetworkObject>();
        gunNetworkObject.Spawn(true);

        GunObject gunObject = gunObjectTransform.GetComponent<GunObject>();

        gunObjectParentNetworkObjectRef.TryGet(out NetworkObject gunObjectParentNetworkObject);
        IGunObjectParent gunObjectParent = gunObjectParentNetworkObject.GetComponent<IGunObjectParent>();
        gunObject.SetGunObjectParent(gunObjectParent);
    }

    public int GetGunObjectSOIndex(GunObjectSO gunObjectSO)
    {
        return _gunObjectListSO.GunObjectsSOList.IndexOf(gunObjectSO);
    }

    public GunObjectSO GetGunObjectSOFromIndex(int gunObjectSOListIndex)
    {
        return _gunObjectListSO.GunObjectsSOList[gunObjectSOListIndex];
    }

    public void DestroyGunObject(GunObject gunObject)
    {
        DestroyGunObjectServerRpc(gunObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyGunObjectServerRpc(NetworkObjectReference gunObjectNetworkBehaviourRef)
    {
        gunObjectNetworkBehaviourRef.TryGet(out NetworkObject gunNetworkObject);
        GunObject gunObject = gunNetworkObject.GetComponent<GunObject>();

        ClearGunObjectOnParentClientRpc(gunObjectNetworkBehaviourRef);
        gunObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearGunObjectOnParentClientRpc(NetworkObjectReference gunObjectNetworkBehaviourRef)
    {
        gunObjectNetworkBehaviourRef.TryGet(out NetworkObject gunNetworkObject);
        GunObject gunObject = gunNetworkObject.GetComponent<GunObject>();

        gunObject.ClearGunObjectOnParent();
    }

    public void SpawnGoldCoinObject(Vector3 gameObjectPosition, Vector2 spawnArea = default)
    {
        SpawnGoldCoinObjectServerRpc(gameObjectPosition, spawnArea);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnGoldCoinObjectServerRpc(Vector3 gameObjectPosition, Vector2 spawnArea = default)
    {

        Transform goldCoinTransform = Instantiate(_goldCoinSO.Prefab, gameObjectPosition + new Vector3(UnityEngine.Random.Range(-spawnArea.x, spawnArea.x), 1, UnityEngine.Random.Range(-spawnArea.y, spawnArea.y)), Quaternion.identity);
        NetworkObject goldCoinNetworkObject = goldCoinTransform.GetComponent<NetworkObject>();
        goldCoinNetworkObject.Spawn(true);
    }

    public bool IsPlayerDataIndexConnected(int playerIndex)
    {
        return playerIndex < _playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientID(ulong clientID)
    {
        for (int i = 0; i < _playerDataNetworkList.Count; i++)
        {
            if (_playerDataNetworkList[i].ClientID == clientID)
            {
                return i;
            }
        }
        return -1;
    }

    public void KickPlayer(ulong clientID)
    {
        if (clientID == NetworkManager.ServerClientId)
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        }
        NetworkManager.Singleton.DisconnectClient(clientID);
        NetworkManager_Server_OnClientDisconnectCallback(clientID);
    }

}
