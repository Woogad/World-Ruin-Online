using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GoldCoin : NetworkBehaviour
{
    [SerializeField] private GoldCoinSO _goldCoinSO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.AddGoldCoin(1);
            DestroySelfServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroySelfServerRpc()
    {
        NetworkObject.Despawn(true);
    }

}
