using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player _player;
    private float _footsetpTimer;
    private float _footsetpTimerMax = .4f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.OnRelaod += PlayerOnReload;
        _player.OnShoot += PlayerOnShoot;
    }

    private void PlayerOnShoot(object sender, EventArgs e)
    {
        float volume = 1f;
        if (_player.GetGunObject().getCurrentAmmo() != 0)
        {
            SoundManager.Instance.PlayGunShootSound(_player.transform.position, volume);
        }
        else
        {
            SoundManager.Instance.PlayEmptyShoot(_player.transform.position, volume);
        }
    }

    private void PlayerOnReload(object sender, EventArgs e)
    {
        float volume = 1f;
        SoundManager.Instance.PlayReloadSound(_player.transform.position, volume);
    }

    private void FixedUpdate()
    {
        _footsetpTimer -= Time.deltaTime;
        if (_footsetpTimer < 0f)
        {
            _footsetpTimer = _footsetpTimerMax;
            if (_player.IsWalking())
            {
                float volume = .5f;
                SoundManager.Instance.PlayFootstepSound(_player.transform.position, volume);
            }
        }

    }
}
