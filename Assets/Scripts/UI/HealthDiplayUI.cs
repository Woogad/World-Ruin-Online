using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthDiplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthAmountText;

    private void Start()
    {
        Player.Instance.OnAddHealth += PlayerOnAddHealth;
        UpdateVisual();
    }

    private void PlayerOnAddHealth(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        healthAmountText.text = Mathf.Round(Player.Instance.GetPlayerHealth()).ToString();
    }
}
