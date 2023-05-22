using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AmmoDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _ammoAmountText;
    [SerializeField] private TextMeshProUGUI _magazineAmountText;


    private void Start()
    {
        _player.OnAmmoChanged += PlayerOnUpdateAmmo;
        _player.OnInteract += PlayerOnInteract;

        UpdateVisual();
    }

    private void PlayerOnInteractClientAmmo(object sender, EventArgs e)
    {
        Invoke("UpdateVisualClient", 0.5f);
    }

    private void PlayerOnInteract(object sender, EventArgs e)
    {
        Debug.Log("Player has gun? " + _player.HasGunObject());
        if (_player.HasGunObject())
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
        if (!_player.HasGunObject())
        {

            _ammoAmountText.text = "0";
            _magazineAmountText.text = "0";
            UpdateVisualLate();
            return;
        }
        _ammoAmountText.text = _player.GetGunObject().getCurrentAmmo().ToString();
        _magazineAmountText.text = _player.GetGunObject().getCurrentMagazine().ToString();
    }

    private void UpdateVisualLate()
    {
        Invoke("UpdateVisual", 0.5f);
    }
}
