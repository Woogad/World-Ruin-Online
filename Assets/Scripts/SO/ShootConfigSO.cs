using UnityEngine;

[CreateAssetMenu()]
public class ShootConfigSO : ScriptableObject
{
    public float Damage;
    public float FireRate;
    public Vector3 Spread;
}
