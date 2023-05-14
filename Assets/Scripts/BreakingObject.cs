using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingObject : MonoBehaviour, IDamageable
{
    [SerializeField] private BreakingObjectSO _breakingObjectSO;
    private float _hp;

    private void Start()
    {
        _hp = _breakingObjectSO.MaxHp;
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
