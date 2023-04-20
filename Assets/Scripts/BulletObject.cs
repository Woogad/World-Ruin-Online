using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    Vector3 shootDir;

    public void Setup(Transform shootDir)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        float _moveSpeed = 60f;
        rigidbody.AddForce(shootDir.forward * _moveSpeed, ForceMode.Impulse);

        float timeDestroySelf = 3f;
        Destroy(gameObject, timeDestroySelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Destroy(gameObject);
        }

        if (other.TryGetComponent(out IDamageable Idamageable))
        {
            Idamageable.TakeDamage();
            Destroy(gameObject);
        }
    }
}
