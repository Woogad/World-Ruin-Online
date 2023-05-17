using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OwnerCanvasPlayerUI : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
    }
}
