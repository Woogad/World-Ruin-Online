using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyAmount;
    private void Start()
    {
        Player.Instance.OnUpdateMoney += PlayerOnUpdateMoney;
        UpdateVisual();
    }

    private void PlayerOnUpdateMoney(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _moneyAmount.text = Player.Instance.GetPlayerMoney().ToString() + "$";
    }
}