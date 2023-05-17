using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Image _damageOverlayImage;
    [SerializeField] private float _duration;
    [SerializeField] private float _fadeSpeed;

    private float _durationTimer;
    void Start()
    {
        _player.OnTakeDamage += PlayerOnTakeDamage;
        _damageOverlayImage.color = new Color(_damageOverlayImage.color.r, _damageOverlayImage.color.g, _damageOverlayImage.color.b, 0);
    }

    private void Update()
    {
        if (_damageOverlayImage.color.a > 0)
        {
            _durationTimer += Time.deltaTime;
            if (_durationTimer > _duration)
            {
                float tempAlpha = _damageOverlayImage.color.a;
                tempAlpha -= Time.deltaTime * _fadeSpeed;
                _damageOverlayImage.color = new Color(_damageOverlayImage.color.r, _damageOverlayImage.color.g, _damageOverlayImage.color.b, tempAlpha);
            }
        }
    }

    private void PlayerOnTakeDamage(object sender, EventArgs e)
    {
        _duration = 0;
        float startOverlayAlpa = 0.6f;
        _damageOverlayImage.color = new Color(_damageOverlayImage.color.r, _damageOverlayImage.color.g, _damageOverlayImage.color.b, startOverlayAlpa);
    }
}
