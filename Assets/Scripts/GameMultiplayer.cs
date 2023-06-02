using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    public event EventHandler OnTryToJoinGame;
    public event EventHandler OnFailToJoinGame;

    [SerializeField] private GunObjectListSO _gunObjectListSO;
    [SerializeField] private GoldCoinSO _goldCoinSO;

    private const int MAX_PLAYER_LIMIT = 4;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManagerConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        OnTryToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManagerOnClientDisconnectCallback(ulong obj)
    {
        OnFailToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    private void NetworkManagerConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
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

    public GunObjectSO GetGunObjectSOFromIndex(int gunObjectSOList)
    {
        return _gunObjectListSO.GunObjectsSOList[gunObjectSOList];
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

}
