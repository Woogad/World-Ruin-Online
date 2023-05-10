using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    private float _damage;

    public void Setup(Transform shootDir, ShootConfigSO shootConfigOS)
    {
        this._damage = shootConfigOS.Damage;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        float _moveSpeed = 60f;
        rigidbody.AddForce(shootDir.forward * _moveSpeed, ForceMode.Impulse);

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
        Idamageable.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
