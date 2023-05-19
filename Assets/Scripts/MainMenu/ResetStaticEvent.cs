using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticEvent : MonoBehaviour
{
    private void Awake()
    {
        ClearCounter.ResetStaticEvent();
        ItemShopCounter.ResetStaticEvent();
        GunShopCounter.ResetStaticEvent();
        Player.ResetStaticEvent();
    }

}
