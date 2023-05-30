using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrader : BaseCounter
{
    public static event EventHandler OnAnyTradeCoin;
    public static event EventHandler OnAnyFailTradeCoin;
    public static void ResetStaticEvent()
    {
        OnAnyTradeCoin = null;
        OnAnyFailTradeCoin = null;
    }

    [SerializeField] private GoldCoinSO _goldCoinSO;
    public override void Interact(Player player)
    {
        if (player.GetGoldCoin() != 0)
        {
            player.AddPlayerMoney(player.GetGoldCoin() * _goldCoinSO.MoneyReward);
            player.AddGoldCoin(-player.GetGoldCoin());
            OnAnyTradeCoin?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnAnyFailTradeCoin?.Invoke(this, EventArgs.Empty);
        }
    }

}
