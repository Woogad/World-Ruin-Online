using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DeadDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _killedByText;

    private void Start()
    {
        _player.OnDead += PlayerOnDead;
        _player.OnReSpawn += PlayerOnReSpawn;
        Hide();
    }

    private void PlayerOnReSpawn(object sender, Player.OnReSpawnArgs e)
    {
        Hide();
    }

    private void PlayerOnDead(object sender, Player.OnDeadArgs e)
    {
        UpdateVisual(e.KillerClientID);
        Show();
    }

    private void UpdateVisual(ulong KillerClientID)
    {
        foreach (var player in GameMultiplayer.Instance.GetPlayerDataNetworkList())
        {
            if (player.ClientID == KillerClientID)
            {
                _killedByText.text = player.PlayerName.ToString();
                break;
            }
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
