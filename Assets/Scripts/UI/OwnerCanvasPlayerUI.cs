using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class OwnerCanvasPlayerUI : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            gameObject.SetActive(false);
        }
        GameManager.Instance.OnStateChanged += GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            gameObject.SetActive(false);
        }
    }
}
