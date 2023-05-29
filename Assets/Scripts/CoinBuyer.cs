using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBuyer : BaseCounter
{
    [SerializeField] private GoldCoinSO _goldCoinSO;
    public override void Interact(Player player)
    {
        if (player.GetGoldCoin() != 0)
        {
            player.AddPlayerMoney(player.GetGoldCoin() * _goldCoinSO.MoneyReward);
            player.AddGoldCoin(-player.GetGoldCoin());
        }
    }

}
