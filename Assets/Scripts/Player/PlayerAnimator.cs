using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;

    private const string IS_WALKING = "IsWalking";
    private const string DRAW_GUN = "DrawGun";
    private const string RELOAD = "Reload";
    private const string IS_SHOOT_AUTO = "IsShootAuto";
    private const string SHOOT_SEMI = "ShootSemi";
    private const string IS_DEAD = "IsDead";
    private const string SPEED_RELAOD_MULTIPLIER_ANIMATOR = "SpeedReloadMultiplierAnimator";

    private Animator _animator;
    private NetworkAnimator _networkAnimator;
    private bool _isHoldShootAction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _networkAnimator = GetComponent<NetworkAnimator>();
    }

    private void Start()
    {
        _player.OnInteract += GunShopOnPlayerBuyGun;
        _player.OnRelaod += PlayerOnReload;
        _player.OnShoot += PlayerOnShoot;
        _player.OnDead += PlayerOnDead;
        _player.OnReSpawn += PlayerOnReSpawn;
        GameInput.Instance.OnShootWeaponHoldAction += GameInputOnShootWeaponHoldAction;
    }

    private void PlayerOnReSpawn(object sender, Player.OnReSpawnArgs e)
    {
        _animator.SetBool(IS_DEAD, false);
    }

    private void PlayerOnDead(object sender, Player.OnDeadArgs e)
    {
        _animator.SetBool(IS_DEAD, true);
        _animator.SetBool(IS_SHOOT_AUTO, false);
    }


    private void PlayerOnShoot(object sender, EventArgs e)
    {
        if (_player.GetGunObject().GetGunMode() != GunObject.GunMode.Semi) return;
        _networkAnimator.SetTrigger(SHOOT_SEMI);

    }

    private void PlayerOnReload(object sender, EventArgs e)
    {
        _isHoldShootAction = false;
        _animator.SetFloat(SPEED_RELAOD_MULTIPLIER_ANIMATOR, 1 / _player.GetGunObject().GetGunObjectSO().ReloadTime * 2);
        _networkAnimator.SetTrigger(RELOAD);
    }

    private void GameInputOnShootWeaponHoldAction(object sender, GameInput.OnShootWeaponActionArgs e)
    {
        _isHoldShootAction = e.IsHoldShootAction;
    }

    private void GunShopOnPlayerBuyGun(object sender, EventArgs e)
    {
        _networkAnimator.SetTrigger(DRAW_GUN);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (_player.HasGunObject())
        {
            if (_player.GetGunObject().GetGunMode() == GunObject.GunMode.Auto)
            {
                _animator.SetBool(IS_SHOOT_AUTO, _isHoldShootAction && _player.GetGunObject().getCurrentAmmo() != 0);
            }
            else
            {
                _animator.SetBool(IS_SHOOT_AUTO, false);
            }
        }
        else
        {
            _animator.SetBool(IS_SHOOT_AUTO, false);
        }
        _animator.SetBool(IS_WALKING, _player.IsWalking());

    }
}
