using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunObject : MonoBehaviour
{
    [SerializeField] private GunObjectOS _gunObjectOS;
    [SerializeField] private Transform _fireEndPoint;

    private IGunObjectParent _gunObjectParent;

    public GunObjectOS GetGunObjectOS()
    {
        return this._gunObjectOS;
    }

    public ShootConfigOS GetShootConfigOS()
    {
        return this.GetGunObjectOS().ShootConfigOS;
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
        return this._gunObjectParent;
    }

    public void DestroySelf()
    {
        _gunObjectParent.ClearGunObject();
        Destroy(gameObject);
    }

    public Transform GetFireEndPointTransform()
    {
        return this._fireEndPoint;
    }
}
