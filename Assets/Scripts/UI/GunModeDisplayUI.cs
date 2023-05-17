using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GunModeDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _gunModeText;

    private void Start()
    {
        _player.OnGunModeChanged += PlayerOnUpdateGunMode;
        UpdateVisual();
    }

    private void PlayerOnUpdateGunMode(object sender, Player.OnGunModeChangedArgs e)
    {
        UpdateVisual(e.GunMode.ToString());
    }

    private void UpdateVisual(string gunMode = "Semi")
    {
        if (_player.HasGunObject())
        {
            _gunModeText.text = gunMode;
        }
    }
}
