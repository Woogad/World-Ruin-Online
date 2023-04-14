using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunObjectParent
{
    public Transform GetGunObjectFollowTransform();

    public void SetGunObject(GunObject gunObject);

    public GunObject GetGunObject();

    public void ClearGunObject();

    public bool HasGunObject();

    public Quaternion GetGunQuaternion();
}
