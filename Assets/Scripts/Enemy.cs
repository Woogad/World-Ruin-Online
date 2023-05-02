using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    //! This is prototype for enemy
    [SerializeField] private GunObject _gunObject;
    [SerializeField] private Transform GunEndPoint;
    private float _shootTimer = 2;
    private float _shootTimerMax = 3;
    private float _health = 50f;

    private void Update()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer < 0f)
        {
            _shootTimer = _shootTimerMax;
            _gunObject.Shoot();
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Take damage!! {damage}");
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
