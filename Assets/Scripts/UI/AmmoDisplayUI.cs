using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AmmoDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoAmountText;
    [SerializeField] private TextMeshProUGUI _magazineAmountText;
    [SerializeField] private TextMeshProUGUI _maxMagazineAmountText;


    private void Start()
    {
        Player.Instance.OnUpdateAmmo += PlayerOnUpdateAmmo;
        Player.Instance.OnInteract += PlayerOnInteract;

        _ammoAmountText.text = "0";
        _magazineAmountText.text = "0";
        _maxMagazineAmountText.text = "0";
    }

    private void PlayerOnInteract(object sender, EventArgs e)
    {
        if (Player.Instance.HasGunObject())
        {
            UpdateVisual();
        }
    }

    private void PlayerOnUpdateAmmo(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _ammoAmountText.text = Player.Instance.GetGunObject().getCurrentAmmo().ToString();
        _magazineAmountText.text = Player.Instance.GetGunObject().getCurrentMagazine().ToString();
        _maxMagazineAmountText.text = Player.Instance.GetGunObject().GetGunObjectSO().MaxMagazine.ToString();
    }
}
