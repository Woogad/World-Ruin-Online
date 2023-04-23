using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunObject : MonoBehaviour
{
    [SerializeField] private GunObjectSO _gunObjectSO;
    [SerializeField] private Transform _fireEndPoint;

    private IGunObjectParent _gunObjectParent;
    private float _cooldownTimestamp = 0;
    private int _currentMagazine;
    private int _currentAmmo;

    public GunObjectSO GetGunObjectSO()
    {
        return this._gunObjectSO;
    }

    public ShootConfigSO GetShootConfigOS()
    {
        return this.GetGunObjectSO().ShootConfigSO;
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
        Transform bulletTransform = Instantiate(_gunObjectSO.BulletPrefab, _fireEndPoint.position, Quaternion.identity);
        bulletTransform.GetComponent<BulletObject>().Setup(_fireEndPoint, GetShootConfigOS());
        _currentAmmo--;
    }

    public void Reload()
    {
        int reloadAmount = _gunObjectSO.MaxAmmmo - _currentAmmo;
        reloadAmount = (_currentMagazine - reloadAmount) >= 0 ? reloadAmount : _currentMagazine;
        _currentAmmo += reloadAmount;
        _currentMagazine -= reloadAmount;
    }

    public void AddMagazine(int magazine)
    {
        _currentMagazine += magazine;
        if (_currentMagazine > _gunObjectSO.MaxMagazine)
        {
            _currentMagazine = _gunObjectSO.MaxMagazine;
        }

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
        _currentAmmo = _gunObjectSO.MaxAmmmo;
        _currentMagazine = _gunObjectSO.MaxMagazine;
    }

    public int getCurrentAmmo()
    {
        return this._currentAmmo;
    }
    public int getCurrentMagazine()
    {
        return this._currentMagazine;
    }

}
