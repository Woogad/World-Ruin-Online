using UnityEngine;

public class BulletObject : MonoBehaviour
{
    private float _damage;

    public void Setup(Transform FireEndPoint, ShootConfigSO shootConfigOS)
    {
        this._damage = shootConfigOS.Damage;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        //* Random Shoot Spread
        Vector3 shootDir = FireEndPoint.forward + new Vector3(
        Random.Range(
            -shootConfigOS.Spread.x,
            shootConfigOS.Spread.x
        ),
        Random.Range(
            -shootConfigOS.Spread.y,
            shootConfigOS.Spread.y
        ),
        Random.Range(
            -shootConfigOS.Spread.z,
             shootConfigOS.Spread.z
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
