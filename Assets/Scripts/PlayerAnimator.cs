using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player _player;

    private const string IS_WALKING = "IsWalking";
    private const string DRAW_GUN = "DrawGun";
    private const string RELOAD = "Reload";
    private const string IS_SHOOT = "IsShoot";

    private Animator _animator;
    private bool _isHoldShootAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player.OnInteract += GunShopOnPlayerBuyGun;
        _player.OnRelaod += PlayerOnReload;
        GameInput.Instance.OnShootWeaponHoldAction += GameInputOnShootWeaponHoldAction;
    }

    private void PlayerOnReload(object sender, EventArgs e)
    {
        _isHoldShootAction = false;
        _animator.SetTrigger(RELOAD);
    }

    private void GameInputOnShootWeaponHoldAction(object sender, GameInput.OnShootWeaponActionArgs e)
    {
        _isHoldShootAction = e.IsHoldShootAction;
    }

    private void GunShopOnPlayerBuyGun(object sender, EventArgs e)
    {
        _animator.SetTrigger(DRAW_GUN);
    }


    private void Update()
    {
        if (_player.HasGunObject())
        {
            _animator.SetBool(IS_SHOOT, _isHoldShootAction && _player.GetGunObject().getCurrentAmmo() != 0);
        }
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}
