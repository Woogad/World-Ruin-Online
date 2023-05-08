using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnReloadAction;
    public event EventHandler OnToggleWeaponModeAction;
    public event EventHandler OnShootWeaponAction;
    public event EventHandler<OnShootWeaponActionArgs> OnShootWeaponHoldAction;
    public class OnShootWeaponActionArgs : EventArgs
    {
        public bool IsHoldShootAction;
    }

    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();

        //*left mouse event
        _playerInputAction.Player.ShootWeapon.started += ShootWeaponStart;
        _playerInputAction.Player.ShootWeapon.performed += ShootWeaponPerformed;
        _playerInputAction.Player.ShootWeapon.canceled += ShootWeaponCanceled;

        _playerInputAction.Player.Interact.performed += InteractPerformed;
        _playerInputAction.Player.ReloadWeapon.performed += ReloadWeaponPerformed;
        _playerInputAction.Player.ToggleWeaponMode.performed += ToggleWeaponModePerformed;
    }


    private void ShootWeaponStart(InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnShootWeaponAction(this, EventArgs.Empty);
    }

    private void ShootWeaponPerformed(InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        bool isHoldShootAction = true;
        OnShootWeaponHoldAction?.Invoke(this, new OnShootWeaponActionArgs
        {
            IsHoldShootAction = isHoldShootAction
        });
    }

    private void ShootWeaponCanceled(InputAction.CallbackContext obj)
    {
        bool isHoldShootAction = false;
        OnShootWeaponHoldAction?.Invoke(this, new OnShootWeaponActionArgs
        {
            IsHoldShootAction = isHoldShootAction
        });
    }

    private void ToggleWeaponModePerformed(InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnToggleWeaponModeAction?.Invoke(this, EventArgs.Empty);
    }

    private void ReloadWeaponPerformed(InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnReloadAction?.Invoke(this, EventArgs.Empty);
    }


    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        if (!GameManager.Instance.IsGamePlaying()) return new Vector2(0, 0);
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }


}
