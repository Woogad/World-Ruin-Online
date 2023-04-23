using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunObjectSO : ScriptableObject
{
    public ShootConfigSO ShootConfigSO;
    public Transform Prefab;
    public Transform BulletPrefab;
    public int MaxMagazine;
    public int MaxAmmmo;
    public int Price;
    public string GunName;

}
