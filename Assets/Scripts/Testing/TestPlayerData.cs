using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class TestPlayerData : NetworkBehaviour
{
    private void Start()
    {
        // if (IsClient)
        // {
        NetworkManager.Singleton.OnClientConnectedCallback += gg;
        // }
    }

    private void gg(ulong clientID)
    {
        if (clientID == NetworkManager.Singleton.LocalClientId)
        {
            // GameMultiplayer.Instance.aa(OwnerClientId, PlayerPrefs.GetInt("PlayerPrefabIndex", 1));
        }
    }
}
