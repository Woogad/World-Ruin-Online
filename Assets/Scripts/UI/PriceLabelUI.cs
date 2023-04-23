using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriceLabelUI : MonoBehaviour
{
    [SerializeField] private GunShopCounter _gunShopCounter;
    [SerializeField] private TextMeshProUGUI _PriceLabelText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _PriceLabelText.text = _gunShopCounter.GetGunObjectOSShop().Price.ToString() + "$";
    }
}
