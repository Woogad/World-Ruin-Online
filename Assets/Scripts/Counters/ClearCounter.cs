using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private GunObjectOS _gunObjectSO;

    private void OnEnable()
    {
        if (_gunObjectSO != null)
        {
            Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
            gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(this);
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
                ClearGunObject();
            }
            else
            //* Player is carrying something
            {
            }
        }

    }

}
