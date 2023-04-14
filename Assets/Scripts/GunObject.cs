using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunObject : MonoBehaviour
{
    [SerializeField] private GunObjectOS _gunObjectOS;

    private IGunObjectParent _gunObjectParent;

    public GunObjectOS GetGunObjectOS()
    {
        return _gunObjectOS;
    }

    public void SetGunObjectParent(IGunObjectParent gunObjectParent)
    {
        if (this._gunObjectParent != null)
        {
            this._gunObjectParent.ClearGunObject();
        }
        if (gunObjectParent.HasGunObject())
        {
            Debug.LogError("IGunObjectParent already has GunObject!");
        }
        this._gunObjectParent = gunObjectParent;
        gunObjectParent.SetGunObject(this);

        transform.parent = gunObjectParent.GetGunObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localRotation = gunObjectParent.GetGunQuaternion();
    }

    public IGunObjectParent GetGunObjectParent()
    {
        return _gunObjectParent;
    }

}
