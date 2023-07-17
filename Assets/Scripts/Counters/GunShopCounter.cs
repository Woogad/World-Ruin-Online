using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShopCounter : BaseCounter
{
    public static event EventHandler OnAnyBuyGun;
    public static event EventHandler OnAnyFailBuyGun;

    public static void ResetStaticEvent()
    {
        OnAnyBuyGun = null;
        OnAnyFailBuyGun = null;
    }

    [SerializeField] private GunObjectSO _gunObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasGunObject())
        //* if player doesn't carrying anything
        {
            if (CanBuyGun(player.GetPlayerMoney()))
            {
                Buy(player);
                GunObject.SpawnGunObject(_gunObjectSO, player);
            }
        }
        else
        //* if player carrying something
        {
            if (CanBuyGun(player.GetPlayerMoney()) && player.GetGunObject().GetGunObjectSO() != this._gunObjectSO)
            {
                Buy(player);
                GunObject.DestroyGunObject(player.GetGunObject());
                GunObject.SpawnGunObject(_gunObjectSO, player);

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
            OnAnyFailBuyGun?.Invoke(this, EventArgs.Empty);
            return false;
        }
    }

    private void Buy(Player player)
    {
        OnAnyBuyGun?.Invoke(this, EventArgs.Empty);
        int playerMoney = player.GetPlayerMoney();
        player.AddPlayerMoney(-_gunObjectSO.Price);
    }

    public GunObjectSO GetGunObjectOSShop()
    {
        return this._gunObjectSO;
    }

}
