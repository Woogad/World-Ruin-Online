using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player _player;

    private const string IS_WALKING = "IsWalking";
    private const string DRAW_GUN = "DrawGun";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _player.OnInteract += GunShopOnPlayerBuyGun;
    }

    private void GunShopOnPlayerBuyGun(object sender, EventArgs e)
    {
        _animator.SetTrigger(DRAW_GUN);
    }

    private void Update()
    {
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}
