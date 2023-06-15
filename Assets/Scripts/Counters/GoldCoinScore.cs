using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoinScore : BaseCounter
{
    public static event EventHandler OnAnyGoldCoinScore;
    public static event EventHandler OnAnyFailCoinScore;

    public static void ResetStaticEvent()
    {
        OnAnyGoldCoinScore = null;
    }

    public override void Interact(Player player)
    {
        if (player.GetGoldCoin() > 0)
        {
            ScoreBoardManager.Instance.AddScoreServerRpc(player.GetGoldCoin());
            player.AddGoldCoin(-player.GetGoldCoin());
            OnAnyGoldCoinScore?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnAnyFailCoinScore?.Invoke(this, EventArgs.Empty);
        }
    }


}
