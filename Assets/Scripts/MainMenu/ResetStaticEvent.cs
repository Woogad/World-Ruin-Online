using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticEvent : MonoBehaviour
{
    private void Awake()
    {
        GoldCoinScore.ResetStaticEvent();
        ItemShopCounter.ResetStaticEvent();
        GunShopCounter.ResetStaticEvent();
        CoinTrader.ResetStaticEvent();
        Player.ResetStaticEvent();
    }

}
