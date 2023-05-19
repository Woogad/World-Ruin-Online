using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    [SerializeField] private GunObjectListSO _gunObjectListSO;

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

        NetworkObject gunObjectNetworkObject = gunObjectTransform.GetComponent<NetworkObject>();
        gunObjectNetworkObject.Spawn(true);

        GunObject gunObject = gunObjectTransform.GetComponent<GunObject>();

        gunObjectParentNetworkObjectRef.TryGet(out NetworkObject gunObjectParentNetworkObject);
        IGunObjectParent gunObjectParent = gunObjectParentNetworkObject.GetComponent<IGunObjectParent>();
        gunObject.SetGunObjectParent(gunObjectParent);
    }

    private int GetGunObjectSOIndex(GunObjectSO gunObjectSO)
    {
        return _gunObjectListSO.GunObjectsSOList.IndexOf(gunObjectSO);
    }

    private GunObjectSO GetGunObjectSOFromIndex(int gunObjectSOList)
    {
        return _gunObjectListSO.GunObjectsSOList[gunObjectSOList];
    }
}
