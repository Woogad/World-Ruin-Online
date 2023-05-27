using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BreakingObject : NetworkBehaviour, IDamageable
{
    [SerializeField] private BreakingObjectSO _breakingObjectSO;
    private NetworkVariable<float> _hp = new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        _hp.Value = _breakingObjectSO.MaxHp;
    }
    public void TakeDamage(float damage, ulong shootOwnerClientID)
    {
        if (IsHost)
        {
            _hp.Value -= damage;
            if (_hp.Value <= 0)
            {
                NetworkObject.Despawn(true);
            }
            return;
        }
        TakeDamageServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(float damage)
    {
        _hp.Value -= damage;
        if (_hp.Value <= 0)
        {
            NetworkObject.Despawn(true);
        }
    }
}
