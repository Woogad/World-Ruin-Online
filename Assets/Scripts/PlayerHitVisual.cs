using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitVisual : MonoBehaviour
{
    [SerializeField] private GameObject _hitPlayerGameObject;

    private void Start()
    {
        // Hide();
        Player.Instance.OnTakeDamage += PlayerOnTakeDamage;
    }

    private void PlayerOnTakeDamage(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        _hitPlayerGameObject.SetActive(true);
    }
    private void Hide()
    {
        _hitPlayerGameObject.SetActive(false);
    }
}
