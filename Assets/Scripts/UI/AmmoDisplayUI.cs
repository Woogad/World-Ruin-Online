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

    private event Action _updateVisualName;


    private void Start()
    {
        _updateVisualName = this.UpdateVisual;

        _player.OnAmmoChanged += PlayerOnUpdateAmmo;
        _player.OnInteract += PlayerOnInteract;

        UpdateVisual();
    }

    private void PlayerOnInteract(object sender, EventArgs e)
    {
        UpdateVisual();
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
        UpdateVisualLate();
    }

    private void UpdateVisualLate()
    {
        Invoke(_updateVisualName.Method.Name, 0.8f);
    }
}
