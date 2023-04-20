using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public void TakeDamage()
    {
        Debug.Log("Take damage!!");
    }
}
