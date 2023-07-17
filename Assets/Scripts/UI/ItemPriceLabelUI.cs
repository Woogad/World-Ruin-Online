using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPriceLabelUI : MonoBehaviour
{
    [SerializeField] private ItemShopCounter _itemShopCounter;
    [SerializeField] private TextMeshProUGUI _priceLabelText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _priceLabelText.text = _itemShopCounter.GetItemObjectSO().Price.ToString() + "$";
    }
}
