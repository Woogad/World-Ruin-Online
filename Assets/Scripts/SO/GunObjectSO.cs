using UnityEngine;

[CreateAssetMenu()]
public class GunObjectSO : ScriptableObject
{
    public Vector2 Damage;
    public float FireRate;
    public Vector3 Spread;
    public Transform Prefab;
    public Transform BulletPrefab;
    public int MaxMagazine;
    public int MaxAmmmo;
    public int Price;
    public string GunName;

}
