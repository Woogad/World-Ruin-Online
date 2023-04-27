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
    bool _isHoldShootAction = false;

    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();

        //*left mouse event
        _playerInputAction.Player.ShootWeapon.started += ShootWeaponOnce;
        _playerInputAction.Player.ShootWeapon.performed += ShootWeaponHold;
        _playerInputAction.Player.ShootWeapon.canceled += ShootWeaponHold;

        _playerInputAction.Player.Interact.performed += InteractPerformed;
        _playerInputAction.Player.ReloadWeapon.performed += ReloadWeaponPerformed;
        _playerInputAction.Player.ToggleWeaponMode.performed += ToggleWeaponModePerformed;
        _isHoldShootAction = false;
    }

    private void ShootWeaponOnce(InputAction.CallbackContext obj)
    {
        OnShootWeaponAction(this, EventArgs.Empty);
    }

    private void ToggleWeaponModePerformed(InputAction.CallbackContext obj)
    {
        OnToggleWeaponModeAction?.Invoke(this, EventArgs.Empty);
    }

    private void ReloadWeaponPerformed(InputAction.CallbackContext context)
    {
        OnReloadAction?.Invoke(this, EventArgs.Empty);
    }

    private void ShootWeaponHold(InputAction.CallbackContext context)
    {
        if (!_isHoldShootAction)
        {
            _isHoldShootAction = true;
        }
        else
        {
            _isHoldShootAction = false;
        }
        OnShootWeaponHoldAction?.Invoke(this, new OnShootWeaponActionArgs
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
