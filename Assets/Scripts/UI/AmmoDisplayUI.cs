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
    [SerializeField] private string _gunName;


    private void Start()
    {
        _player.OnAmmoChanged += PlayerOnUpdateAmmo;
        _player.OnInteract += PlayerOnInteract;
        UpdateVisual();
    }

    private void PlayerOnInteract(object sender, EventArgs e)
    {
        UpdateVisual();
        StartCoroutine(LateUpdateVisual());
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
            return;
        }
        Debug.Log("Update Ammo");

        int currentAmmo = _player.GetGunObject().GetCurrentAmmo();

        _gunName = _player.GetGunObject().GetGunObjectSO().GunName;
        _ammoAmountText.text = currentAmmo.ToString();
        _magazineAmountText.text = _player.GetGunObject().GetCurrentMagazine().ToString();
        LowAmmoVisual(currentAmmo);
    }

    private IEnumerator LateUpdateVisual()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Late Update Ammo!");

        if (_player.HasGunObject())
        {
            string tempGunName = _player.GetGunObject().GetGunObjectSO().GunName;

            if (_gunName != tempGunName)
            {
                UpdateVisual();
            }
        }
        else
        {
            UpdateVisual();
        }
    }

    private void LowAmmoVisual(int ammo)
    {
        if (ammo < 10)
        {
            _ammoAmountText.color = Color.red;
        }
        else
        {
            _ammoAmountText.color = Color.white;
        }
    }
}
