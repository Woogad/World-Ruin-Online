using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public interface IGunObjectParent
{
    public Transform GetGunObjectFollowTransform();

    public void SetGunObject(GunObject gunObject);

    public GunObject GetGunObject();

    public void ClearGunObject();

    public bool HasGunObject();

    public Vector3 GetLocalScale();

    public NetworkObject GetNetworkObject();
}
