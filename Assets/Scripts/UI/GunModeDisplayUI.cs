using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GunModeDisplayUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _gunModeText;

    private void Start()
    {
        Player.Instance.OnGunModeChanged += PlayerOnUpdateGunMode;
        UpdateVisual();
    }

    private void PlayerOnUpdateGunMode(object sender, Player.OnGunModeChangedArgs e)
    {
        UpdateVisual(e.GunMode.ToString());
    }

    private void UpdateVisual(string gunMode = "Semi")
    {
        if (Player.Instance.HasGunObject())
        {
            _gunModeText.text = gunMode;
        }
    }
}
