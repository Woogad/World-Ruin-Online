using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShopCounter : BaseCounter, IGunObjectParent
{
    [SerializeField] private GunObjectOS _gunObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasGunObject())
        //* if player doesn't carrying object
        {
            if (CanBuyGun(player))
            {
                Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
                gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(player);
            }
        }
        else
        //* if player carrying object
        {
            if (CanBuyGun(player))
            {
                player.GetGunObject().DestroySelf();
                Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
                gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(player);
            }

        }
    }

    private bool CanBuyGun(Player player)
    {
        int playerMoney = player.GetPlayerMoney();
        if (playerMoney >= _gunObjectSO.Price)
        {
            player.SetPlayerMoney(playerMoney - _gunObjectSO.Price);
            return true;
        }
        else
        {
            return false;
        }
    }

}
