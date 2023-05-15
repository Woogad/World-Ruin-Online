using UnityEngine;

[CreateAssetMenu()]
public class GunObjectSO : ScriptableObject
{
    [Header("ShootConfigs")]
    public Vector2 Damage;
    public float FireRate;
    public Vector3 Spread;
    public float ReloadTime;
    [Header("Prefabs")]
    public Transform Prefab;
    public Transform BulletPrefab;
    [Header("Ammo")]
    public int MaxAmmmo;
    [Header("Magazine")]
    public int MaxMagazine;
    [Header("Details")]
    public int Price;
    public string GunName;

}
