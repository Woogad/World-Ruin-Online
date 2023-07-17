using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProtectionUI : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Start()
    {
        _player.OnReSpawn += PlayerOnReSpawn;
        Hide();
    }

    private void PlayerOnReSpawn(object sender, Player.OnReSpawnArgs e)
    {
        if (e.IsProtection)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
