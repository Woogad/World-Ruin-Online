using UnityEngine;

public class BulletObject : MonoBehaviour
{
    private float _damage;

    public void Setup(Transform FireEndPoint, GunObjectSO gunObjectSO)
    {
        this._damage = Mathf.Round(Random.Range(gunObjectSO.Damage.x, gunObjectSO.Damage.y));
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        //* Random Shoot Spread
        Vector3 shootDir = FireEndPoint.forward + new Vector3(
        Random.Range(
            -gunObjectSO.Spread.x,
            gunObjectSO.Spread.x
        ),
        Random.Range(
            -gunObjectSO.Spread.y,
            gunObjectSO.Spread.y
        ),
        Random.Range(
            -gunObjectSO.Spread.z,
             gunObjectSO.Spread.z
        ));
        shootDir.Normalize();

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
        Idamageable.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
