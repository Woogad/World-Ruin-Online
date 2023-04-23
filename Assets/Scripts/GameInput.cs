using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnReloadAction;
    public event EventHandler<OnShootWeaponActionArgs> OnShootWeaponAction;
    public class OnShootWeaponActionArgs : EventArgs
    {
        public bool IsHoldShootAction;
    }
    bool _isHoldShootAction = false;

    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();
        _playerInputAction.Player.Interact.performed += InteractPerformed;
        _playerInputAction.Player.ShootWeapon.performed += ShootWeaponPerformed;
        _playerInputAction.Player.ShootWeapon.canceled += ShootWeaponPerformed;
        _playerInputAction.Player.ReloadWeapon.performed += ReloadWeaponPerformed;
        _isHoldShootAction = false;
    }

    private void ReloadWeaponPerformed(InputAction.CallbackContext context)
    {
        OnReloadAction?.Invoke(this, EventArgs.Empty);
    }

    private void ShootWeaponPerformed(InputAction.CallbackContext context)
    {
        if (!_isHoldShootAction)
        {
            _isHoldShootAction = true;
        }
        else
        {
            _isHoldShootAction = false;
        }
        OnShootWeaponAction?.Invoke(this, new OnShootWeaponActionArgs
        {
            IsHoldShootAction = this._isHoldShootAction
        });
    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }


}
