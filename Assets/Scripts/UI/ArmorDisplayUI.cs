using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ArmorDisplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _aromrAmountText;

    private void Start()
    {
        Player.Instance.OnAddArmor += PlayerOnAddArmor;
        UpdateVisual();
    }

    private void PlayerOnAddArmor(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        float armor = Player.Instance.GetPlayerArmor();
        _aromrAmountText.text = armor.ToString();
    }

}
