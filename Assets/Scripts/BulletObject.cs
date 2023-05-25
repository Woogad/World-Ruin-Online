using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class BulletObject : MonoBehaviour
{
    private float _damage;
    private ulong _shootOwnerClientID;

    public void Setup(GunObject gunObject, Vector3 shootDir, ulong shootOwnerClientID)
    {
        _shootOwnerClientID = shootOwnerClientID;
        this._damage = Mathf.Round(Random.Range(gunObject.GetGunObjectSO().Damage.x, gunObject.GetGunObjectSO().Damage.y));
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        float _moveSpeed = 90f;
        rigidbody.AddForce(shootDir * _moveSpeed, ForceMode.Impulse);

        float timeDestroySelf = 1f;
        Destroy(gameObject, timeDestroySelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable Idamageable))
        {
            Destroy(gameObject);
            return;
        }
        Idamageable.TakeDamage(_damage, _shootOwnerClientID);
        Destroy(gameObject);
    }
}
