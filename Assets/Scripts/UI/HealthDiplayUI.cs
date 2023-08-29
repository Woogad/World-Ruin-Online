using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthDiplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _healthAmountText;

    private void Start()
    {
        _player.OnHealthChanged += PlayerOnHealthChanged;
        UpdateVisual();
    }

    private void PlayerOnHealthChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _healthAmountText.text = Mathf.Round(_player.GetPlayerHealth()).ToString("F0");
    }
}
