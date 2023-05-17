using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ArmorDisplayUI : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] TextMeshProUGUI _aromrAmountText;

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
        _aromrAmountText.text = armor.ToString();
    }

}
