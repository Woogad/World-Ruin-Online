using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoldCoinDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _goldCoinAmountText;

    private void Start()
    {
        UpdateVisual();
        _player.OnGoldCoinChanged += PlayerOnGoldCoinChanged;
    }

    private void PlayerOnGoldCoinChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _goldCoinAmountText.text = _player.GetGoldCoin().ToString();
    }
}
