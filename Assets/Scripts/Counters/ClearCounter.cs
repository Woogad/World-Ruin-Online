using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public static event EventHandler OnAnyClearCounterPickObject;
    public static void ResetStaticEvent()
    {
        OnAnyClearCounterPickObject = null;
    }

    [SerializeField] private GunObjectSO _gunObjectSO;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (_gunObjectSO != null)
        {
            //TODO get gunObject when spawned
        }
    }


    public override void Interact(Player player)
    {

        if (HasGunObject())
        //* There is gunObject On counter
        {
            if (!player.HasGunObject())
            //* Player not carrying anything
            {
                GetGunObject().SetGunObjectParent(player);
            }
            else
            //* Player is carrying something
            {
                // if (player.GetGunObject().GetGunObjectSO() == this._gunObjectSO)
                // //* Player has same gun
                // {
                //     player.GetGunObject().AddMagazine(_gunObjectSO.MaxMagazine / 2);
                //     OnAnyClearCounterPickObject?.Invoke(this, EventArgs.Empty);
                //     GetGunObject().DestroySelf();
                // }
                // else
                // //* Player has different gun
                // {
                //     player.GetGunObject().DestroySelf();
                //     GetGunObject().SetGunObjectParent(player);
                // }
            }
        }
        else
        {
            // if (player.HasGunObject())
            // {
            //     player.GetGunObject().SetGunObjectParent(this);
            // }
        }
    }


}
