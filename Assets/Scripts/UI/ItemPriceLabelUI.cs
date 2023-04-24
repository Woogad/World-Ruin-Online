using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPriceLabelUI : MonoBehaviour
{
    [SerializeField] ItemShopCounter _itemShopCounter;
    [SerializeField] TextMeshProUGUI _priceLabelText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _priceLabelText.text = _itemShopCounter.GetItemObjectSO().Price.ToString() + "$";
    }
}
