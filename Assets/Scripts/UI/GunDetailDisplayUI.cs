using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GunDetailDisplayUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _gunModeText;
    [SerializeField] private TextMeshProUGUI _gunNameText;

    private void Start()
    {
        _player.OnGunModeChanged += PlayerOnUpdateGunMode;
        UpdateVisual();
    }

    private void PlayerOnUpdateGunMode(object sender, Player.OnGunModeChangedArgs e)
    {
        UpdateVisual(e.GunMode.ToString());
    }

    private void UpdateVisual(string gunMode = "None")
    {
        if (_player.HasGunObject())
        {
            _gunNameText.text = _player.GetGunObject().GetGunObjectSO().GunName;
            _gunModeText.text = gunMode;
        }
        else
        {

            _gunNameText.text = "---";
            _gunModeText.text = gunMode;
        }
    }
}
