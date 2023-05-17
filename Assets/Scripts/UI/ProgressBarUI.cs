using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Components;

public class ProgressBarUI : NetworkBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _barImage;

    private void Start()
    {
        _player.OnReloadProgressChanged += PlayerOnReloadProgress;
        _barImage.fillAmount = 0f;
        Hide();
    }

    private void PlayerOnReloadProgress(object sender, Player.OnReloadProgressChangedArgs e)
    {
        if (!IsOwner) return;
        if (IsHost)
        {
            ReloadProgressBarUIClientRpc(e.ReloadProgressNormalized);
        }
        ReloadProgressBarUIServerRpc(e.ReloadProgressNormalized);
    }

    [ClientRpc]
    private void ReloadProgressBarUIClientRpc(float ReloadProgressNormalized)
    {
        // ReloadProgressBarUIServerRpc(ReloadProgressNormalized);
        _barImage.fillAmount = ReloadProgressNormalized;
        if (_barImage.fillAmount == 0f || _barImage.fillAmount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    [ServerRpc]
    private void ReloadProgressBarUIServerRpc(float ReloadProgressNormalized)
    {
        ReloadProgressBarUIClientRpc(ReloadProgressNormalized);
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
