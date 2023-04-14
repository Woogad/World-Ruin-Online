using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShopCounter : BaseCounter, IGunObjectParent
{
    [SerializeField] private GunObjectOS _gunObjectSO;

    public override void Interact(Player player)
    {
        Transform gunObjectSOTransform = Instantiate(_gunObjectSO.Prefab);
        gunObjectSOTransform.GetComponent<GunObject>().SetGunObjectParent(player);
    }

}
