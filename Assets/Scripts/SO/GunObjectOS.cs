using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunObjectOS : ScriptableObject
{

    public ShootConfigOS ShootConfigOS;
    public Transform Prefab;
    public string GunName;
    public int Price;
}
