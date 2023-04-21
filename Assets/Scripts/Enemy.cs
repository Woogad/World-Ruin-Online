using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    //! This is prototype for enemy

    private float _health = 50f;
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
