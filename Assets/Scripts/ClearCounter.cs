using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IGunObjectParent
{
    [SerializeField] private GunObjectOS _gunObjectSO;

    public override void Interact(Player player)
    {
        Debug.Log("Clear Counter");
    }

}
