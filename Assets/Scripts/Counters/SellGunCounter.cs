using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellGunCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (player.HasGunObject())
        {
            int soldPrice = player.GetGunObject().GetGunObjectSO().Price / 2;
            player.AddPlayerMoney(soldPrice);
            Debug.Log("sell " + player.GetGunObject().GetGunObjectSO().GunName);
            GunObject.DestroyGunObject(player.GetGunObject());
        }
    }
}
