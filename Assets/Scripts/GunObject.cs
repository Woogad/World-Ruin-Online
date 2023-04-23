using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunObject : MonoBehaviour
{
    [SerializeField] private GunObjectOS _gunObjectOS;
    [SerializeField] private Transform _fireEndPoint;

    private IGunObjectParent _gunObjectParent;
    private float _cooldownTimestamp = 0;
    private int _currentMagazine;
    private int _currentAmmo;

    public GunObjectOS GetGunObjectOS()
    {
        return this._gunObjectOS;
    }

    public ShootConfigOS GetShootConfigOS()
    {
        return this.GetGunObjectOS().ShootConfigOS;
    }

    public bool TryShoot()
    {
        if (Time.time > _cooldownTimestamp && _currentAmmo != 0)
        {
            _cooldownTimestamp = Time.time + GetShootConfigOS().FireRate;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Shoot()
    {
        Transform bulletTransform = Instantiate(_gunObjectOS.BulletPrefab, _fireEndPoint.position, Quaternion.identity);
        bulletTransform.GetComponent<BulletObject>().Setup(_fireEndPoint, GetShootConfigOS());
        _currentAmmo--;
        Debug.Log($"OnShoot= Ammo: {_currentAmmo} | Magazine: {_currentMagazine}");
    }

    public void Reload()
    {
        int reloadAmount = _gunObjectOS.MaxAmmmo - _currentAmmo;
        reloadAmount = (_currentMagazine - reloadAmount) >= 0 ? reloadAmount : _currentMagazine;
        _currentAmmo += reloadAmount;
        _currentMagazine -= reloadAmount;
        Debug.Log($"OnReload= Ammo: {_currentAmmo} | Magazine: {_currentMagazine}");
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

        SetAmmoAndMagazineMax();
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

    private void SetAmmoAndMagazineMax()
    {
        _currentAmmo = _gunObjectOS.MaxAmmmo;
        _currentMagazine = _gunObjectOS.MaxMagazine;
    }

}
