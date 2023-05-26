using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellGunCounter : BaseCounter
{
    private int _soldPrice;

    public override void Interact(Player player)
    {
        if (player.HasGunObject())
        {
            _soldPrice = player.GetGunObject().GetGunObjectSO().Price / 2;
            player.AddPlayerMoney(_soldPrice);
            Debug.Log("sell " + player.GetGunObject().GetGunObjectSO().GunName);
            GunObject.DestroyGunObject(player.GetGunObject());
        }
    }
}
