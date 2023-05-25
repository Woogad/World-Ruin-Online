using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GunObject : NetworkBehaviour
{
    public event EventHandler OnShoot;
    public enum GunMode
    {
        Auto,
        Semi
    }

    [SerializeField] private GunObjectSO _gunObjectSO;
    [SerializeField] private Transform _fireEndPoint;

    private IGunObjectParent _gunObjectParent;
    private float _cooldownTimestamp = 0;
    private int _currentMagazine;
    private int _currentAmmo;
    private GunMode _gunMode = GunMode.Semi;
    private bool _isReload = false;
    private FollowTransform _followTransform;
    private Vector3 _shootRandomVector;

    private void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }

    public GunObjectSO GetGunObjectSO()
    {
        return this._gunObjectSO;
    }


    public bool TryShoot()
    {
        if (_isReload) return false;
        if (Time.time > _cooldownTimestamp && _currentAmmo != 0)
        {
            _cooldownTimestamp = Time.time + _gunObjectSO.FireRate;
            return true;
        }
        else
        {
            return false;
        }
    }

    public GunMode GetGunMode()
    {
        return this._gunMode;
    }

    public GunMode CycleGunMode()
    {
        _gunMode = ((int)_gunMode < 1) ? _gunMode + 1 : 0;
        return this._gunMode;
    }

    public void Shoot(ulong shootOwnerClientID)
    {
        SpawnBulletObjectServerRpc(shootOwnerClientID);
        ShootLogicEventServerRpc();
        _currentAmmo--;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletObjectServerRpc(ulong shootOwnerClientID)
    {
        SpawnBulletObjectClientRpc(shootOwnerClientID);
    }

    [ClientRpc]
    private void SpawnBulletObjectClientRpc(ulong shootOwnerClientID)
    {
        Transform bulletObjectTransform = Instantiate(_gunObjectSO.BulletPrefab, _fireEndPoint.position, Quaternion.identity);
        _shootRandomVector = GetRandomShootNormalize(_fireEndPoint);
        bulletObjectTransform.GetComponent<BulletObject>().Setup(this, _shootRandomVector, shootOwnerClientID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootLogicEventServerRpc()
    {
        ShootLogicEventClientRpc();
    }

    [ClientRpc]
    private void ShootLogicEventClientRpc()
    {
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetRandomShootNormalize(Transform fireEndPoint)
    {
        Vector3 shootDir = _fireEndPoint.forward + new Vector3(
       UnityEngine.Random.Range(
           -_gunObjectSO.Spread.x,
           _gunObjectSO.Spread.x
       ),
       UnityEngine.Random.Range(
           -_gunObjectSO.Spread.y,
           _gunObjectSO.Spread.y
       ),
       UnityEngine.Random.Range(
           -_gunObjectSO.Spread.z,
            _gunObjectSO.Spread.z
       ));
        shootDir.Normalize();

        return shootDir;
    }

    public IEnumerator ReloadTimeCoroutine()
    {
        if (!_isReload && _currentMagazine != 0)
        {
            _isReload = true;
            yield return new WaitForSeconds(_gunObjectSO.ReloadTime);
            Reload();
            _isReload = false;
        }
    }

    public bool IsReload()
    {
        return this._isReload;
    }

    public void Reload()
    {
        if (_currentMagazine != 0)
        {
            Debug.Log("reload");
            int reloadAmount = _gunObjectSO.MaxAmmmo - _currentAmmo;
            reloadAmount = (_currentMagazine - reloadAmount) >= 0 ? reloadAmount : _currentMagazine;
            _currentAmmo += reloadAmount;
            _currentMagazine -= reloadAmount;
        }
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
        if (IsHost)
        {
            SetGunObjectParentClientRpc(gunObjectParent.GetNetworkObject());
        }
        else
        {

            SetGunObjectParentServerRpc(gunObjectParent.GetNetworkObject());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetGunObjectParentServerRpc(NetworkObjectReference gunObjectParentNetworkObjectRef)
    {
        SetGunObjectParentClientRpc(gunObjectParentNetworkObjectRef);
    }

    [ClientRpc]
    private void SetGunObjectParentClientRpc(NetworkObjectReference gunObjectParentNetworkObjectRef)
    {
        gunObjectParentNetworkObjectRef.TryGet(out NetworkObject gunObjectParentNetworkObject);
        IGunObjectParent gunObjectParent = gunObjectParentNetworkObject.GetComponent<IGunObjectParent>();

        if (this._gunObjectParent != null)
        {
            this._gunObjectParent.ClearGunObject();
        }

        this._gunObjectParent = gunObjectParent;

        if (gunObjectParent.HasGunObject())
        {
            Debug.LogError("IGunObjectParent already has GunObject!");
        }

        gunObjectParent.SetGunObject(this);

        _followTransform.SetTargetTransform(gunObjectParent.GetGunObjectFollowTransform(), gunObjectParent.GetLocalScale());
        SetupAmmoAndMagazine();
    }

    public IGunObjectParent GetGunObjectParent()
    {
        return this._gunObjectParent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearGunObjectOnParent()
    {

        _gunObjectParent.ClearGunObject();
    }

    private void SetupAmmoAndMagazine()
    {
        _currentAmmo = _gunObjectSO.MaxAmmmo;
        _currentMagazine = _gunObjectSO.MaxMagazine / 2;
    }

    public int getCurrentAmmo()
    {
        return this._currentAmmo;
    }
    public int getCurrentMagazine()
    {
        return this._currentMagazine;
    }

    public static void SpawnGunObject(GunObjectSO gunObjectSO, IGunObjectParent gunObjectParent)
    {
        GameMultiplayer.Instance.SpawnGunObject(gunObjectSO, gunObjectParent);
    }

    public static void DestroyGunObject(GunObject gunObject)
    {
        GameMultiplayer.Instance.DestroyGunObject(gunObject);
    }
}
