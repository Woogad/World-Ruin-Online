using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopCounter : BaseCounter
{
    public static event EventHandler OnAnyBuyItem;
    public static event EventHandler OnAnyFailBuyItem;

    public static void ResetStaticEvent()
    {
        OnAnyBuyItem = null;
        OnAnyFailBuyItem = null;
    }

    [SerializeField] private ItemObjectSO _itemObjectSO;

    public override void Interact(Player player)
    {
        if (CanBuyItem(player))
        {
            switch (_itemObjectSO.Type)
            {
                case ItemType.ItemTypeList.Health:
                    Buy(player);
                    player.AddPlayerHealth(_itemObjectSO.ItemValue);
                    break;

                case ItemType.ItemTypeList.Armor:
                    Buy(player);
                    player.AddPlayerArmor(_itemObjectSO.ItemValue);
                    break;

                case ItemType.ItemTypeList.Megazine:
                    Buy(player);
                    player.GetGunObject().AddMagazine(Convert.ToInt16(_itemObjectSO.ItemValue));
                    break;
            }
        }
        else
        {
            Debug.LogWarning("ItemShopCounter: Can't buy item!");
            OnAnyFailBuyItem?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CanBuyItem(Player player)
    {
        int playerMoney = player.GetPlayerMoney();

        if (playerMoney >= _itemObjectSO.Price)
        {
            bool CanBuy = true;
            switch (_itemObjectSO.Type)
            {
                case ItemType.ItemTypeList.Health:
                    float playerHealth = player.GetPlayerHealth();
                    if (playerHealth == player.GetPlayerSO().MaxHealth)
                    {
                        CanBuy = false;
                    }
                    break;

                case ItemType.ItemTypeList.Armor:
                    float playerArmor = player.GetPlayerArmor();
                    if (playerArmor == player.GetPlayerSO().MaxArmor)
                    {
                        CanBuy = false;
                    }
                    break;

                case ItemType.ItemTypeList.Megazine:
                    if (!player.HasGunObject())
                    {
                        CanBuy = false;
                        break;
                    }
                    int currentMagazine = player.GetGunObject().GetCurrentMagazine();
                    if (currentMagazine == player.GetGunObject().GetGunObjectSO().MaxMagazine)
                    {
                        CanBuy = false;
                    }
                    break;

                default:
                    Debug.LogError("No item in list");
                    CanBuy = false;
                    break;
            }
            return CanBuy;
        }
        else
        {
            return false;
        }
    }

    private void Buy(Player player)
    {
        OnAnyBuyItem?.Invoke(this, EventArgs.Empty);
        int playerMoney = player.GetPlayerMoney();
        player.AddPlayerMoney(-_itemObjectSO.Price);
    }

    public ItemObjectSO GetItemObjectSO()
    {
        return this._itemObjectSO;
    }
}
