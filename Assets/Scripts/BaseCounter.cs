using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IGunObjectParent
{
    [SerializeField] private Transform _counterTopPoint;


    private GunObject _gunObject;

    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact() called!");
    }

    public Transform GetGunObjectFollowTransform()
    {
        return _counterTopPoint;
    }

    public void SetGunObject(GunObject gunObject)
    {
        this._gunObject = gunObject;
    }

    public GunObject GetGunObject()
    {
        return this._gunObject;
    }

    public void ClearGunObject()
    {
        this._gunObject = null;
    }

    public bool HasGunObject()
    {
        return _gunObject != null;
    }

    public Quaternion GetGunQuaternion()
    {
        return Quaternion.Euler(90, 0, 0);
    }
}
