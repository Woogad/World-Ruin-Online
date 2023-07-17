using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ArmorDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _aromrAmountText;

    private void Start()
    {
        _player.OnArmorChanged += PlayerOnAddArmor;
        UpdateVisual();
    }

    private void PlayerOnAddArmor(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        float armor = _player.GetPlayerArmor();
        _aromrAmountText.text = armor.ToString("F0");
    }

}
