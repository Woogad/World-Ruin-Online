using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunLabelUI : MonoBehaviour
{
    [SerializeField] private GunShopCounter _gunShopCounter;
    [SerializeField] private TextMeshProUGUI _priceLabelText;
    [SerializeField] private TextMeshProUGUI _nameLabelText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _priceLabelText.text = _gunShopCounter.GetGunObjectOSShop().Price.ToString() + "$";
        _nameLabelText.text = _gunShopCounter.GetGunObjectOSShop().GunName;
    }
}
