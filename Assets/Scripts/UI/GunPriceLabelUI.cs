using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunPriceLabelUI : MonoBehaviour
{
    [SerializeField] private GunShopCounter _gunShopCounter;
    [SerializeField] private TextMeshProUGUI _priceLabelText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _priceLabelText.text = _gunShopCounter.GetGunObjectOSShop().Price.ToString() + "$";
    }
}
