using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShopCounter : BaseCounter
{
    [SerializeField] private GunObjectSO _gunObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasGunObject())
        //* if player doesn't carrying object
        {
            if (CanBuyGun(player.GetPlayerMoney()))
            {
                Buy(player);
                Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
                gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(player);
            }
        }
        else
        //* if player carrying object
        {
            if (CanBuyGun(player.GetPlayerMoney()) && player.GetGunObject().GetGunObjectSO() != this._gunObjectSO)
            {
                Buy(player);
                player.GetGunObject().DestroySelf();
                Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
                gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(player);
            }

        }
    }

    private bool CanBuyGun(int money)
    {
        if (money >= _gunObjectSO.Price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Buy(Player player)
    {
        int playerMoney = player.GetPlayerMoney();
        player.AddPlayerMoney(-_gunObjectSO.Price);
    }

    public GunObjectSO GetGunObjectOSShop()
    {
        return this._gunObjectSO;
    }

}
