using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _moneyAmount;

    private void Start()
    {
        _player.OnMoneyChanged += PlayerOnUpdateMoney;
        UpdateVisual();
    }

    private void PlayerOnUpdateMoney(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _moneyAmount.text = _player.GetPlayerMoney().ToString() + "$";
    }
}