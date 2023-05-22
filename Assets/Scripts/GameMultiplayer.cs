using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    [SerializeField] private GunObjectListSO _gunObjectListSO;
    [SerializeField] private Transform _bulletGameObject;
    [SerializeField] private Transform _testPosition;

    private void Awake()
    {
        Instance = this;
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

}
