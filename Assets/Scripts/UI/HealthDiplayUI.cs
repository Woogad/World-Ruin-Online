using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthDiplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI healthAmountText;

    private void Start()
    {
        _player.OnHealthChanged += PlayerOnAddHealth;
        UpdateVisual();
    }

    private void PlayerOnAddHealth(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        healthAmountText.text = Mathf.Round(_player.GetPlayerHealth()).ToString("F0");
    }
}
