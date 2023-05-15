using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGunObjectParent, IDamageable
{
    public static Player Instance { get; private set; }

    public event EventHandler OnInteract;
    public event EventHandler OnAmmoChanged;
    public event EventHandler OnMoneyChanged;
    public event EventHandler OnRelaod;
    public event EventHandler OnShoot;
    public event EventHandler OnArmorChanged;
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;
    public event EventHandler OnPickGun;
    public event EventHandler OnTakeDamage;

    public event EventHandler<OnReloadProgressChangedArgs> OnReloadProgressChanged;
    public class OnReloadProgressChangedArgs : EventArgs
    {
        public float ReloadProgressNormalized;
    }
    public event EventHandler<OnGunModeChangedArgs> OnGunModeChanged;
    public class OnGunModeChangedArgs : EventArgs
    {
        public GunObject.GunMode GunMode;
    }
    public event EventHandler<OnSelectedCoutnerChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCoutnerChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private PlayerSO _playerSO;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _gunObjectHoldPoint;
    [SerializeField] private Mouse3D _mouse3D;

    private float _playerHealth;
    private float _playerArmor;
    private int _playerMoney;
    private float defaultHealth = 60f;
    private float defaultAromr = 0f;
    private int defaulMoney = 500;

    private bool _isHoldShootAction;
    private bool _isAlive;
    private bool _isWalking;
    private float _reloadCountdown;
    private BaseCounter _selectedCounter;
    private GunObject _gunObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance!");
        }
        Instance = this;

        PlayerSetup(defaultHealth, defaultAromr, defaulMoney);
        _isAlive = true;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInputOnInteractAction;
        _gameInput.OnShootWeaponHoldAction += GameInputOnShootAutoAction;
        _gameInput.OnShootWeaponAction += GameInputOnShootAction;
        _gameInput.OnReloadAction += GameInputOnReloadAction;
        _gameInput.OnToggleWeaponModeAction += GameInputOnToggleWeaponModeAction;
    }


    private void Update()
    {
        if (!_isAlive) return;

        if (HasGunObject())
        {
            if (GetGunObject().IsReload())
            {
                _reloadCountdown += Time.deltaTime;
                OnReloadProgressChanged?.Invoke(this, new OnReloadProgressChangedArgs
                {
                    ReloadProgressNormalized = _reloadCountdown / GetGunObject().GetReloadTime()
                });
            }
            else
            {
                if (_reloadCountdown != 0)
                {
                    _reloadCountdown = 0;
                    OnAmmoChanged?.Invoke(this, EventArgs.Empty);
                }

            }

            if (CanShootAuto())
            {
                GetGunObject().Shoot();
                OnShoot?.Invoke(this, EventArgs.Empty);
                OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        HandleMovement();

        //* Make Player lookAt mouse
        transform.LookAt(_mouse3D.GetAngleAimRotate());

        HandleInteraction();
    }

    private void GameInputOnShootAction(object sender, EventArgs e)
    {
        if (HasGunObject() && _isAlive)
        {
            if (GetGunObject().GetGunMode() != GunObject.GunMode.Semi) return;
            if (GetGunObject().TryShoot())
            {
                GetGunObject().Shoot();
                OnShoot?.Invoke(this, EventArgs.Empty);
                OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    private void GameInputOnToggleWeaponModeAction(object sender, EventArgs e)
    {
        if (HasGunObject() && _isAlive)
        {
            GunObject.GunMode gunModeCycle = GetGunObject().CycleGunMode();
            OnGunModeChanged(this, new OnGunModeChangedArgs
            {
                GunMode = gunModeCycle
            });
        }
    }

    private void GameInputOnReloadAction(object sender, EventArgs e)
    {
        if (HasGunObject() && _isAlive)
        {
            if (CanReload())
            {
                StartCoroutine(GetGunObject().ReloadTimeCoroutine());
                OnRelaod?.Invoke(this, EventArgs.Empty);
                float reloadProgress = GetGunObject().GetReloadTime() + 0.01f;
            }
        }
    }

    private void GameInputOnShootAutoAction(object sender, GameInput.OnShootWeaponActionArgs e)
    {
        _isHoldShootAction = e.IsHoldShootAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CanShootAuto()
    {
        if (GetGunObject().GetGunMode() != GunObject.GunMode.Auto) return false;
        if (!GetGunObject().TryShoot()) return false;
        if (!_isHoldShootAction) return false;
        return true;
    }

    private bool CanReload()
    {
        if (GetGunObject().getCurrentAmmo() == GetGunObject().GetGunObjectSO().MaxAmmmo) return false;
        if (GetGunObject().IsReload()) return false;
        if (GetGunObject().getCurrentMagazine() == 0) return false;
        return true;
    }

    private void PlayerSetup(float health, float armor, int money)
    {
        if (health > _playerSO.MaxHealth || armor > _playerSO.MaxArmor || money > _playerSO.MaxMoney)
        {
            Debug.LogError("PlayerSetup is out of limit!");
        }
        this._playerHealth = health;
        this._playerArmor = armor;
        this._playerMoney = money;
    }

    private void Dead()
    {
        _isAlive = false;
        OnReloadProgressChanged?.Invoke(this, new OnReloadProgressChangedArgs
        {
            ReloadProgressNormalized = 0
        });
        _isWalking = false;
        ClearGunObject();
        Collider playerCollider = GetComponent<Collider>();
        playerCollider.enabled = false;
    }

    private void HandleInteraction()
    {

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, interactDistance, _counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);

                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = GetMovementSpeed() * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.6f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        //* Cannot move towards moveDir

        //* Try only x movement
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //* Can move only in x
                moveDir = moveDirX;
            }
            else
            {
                //* Cannot move only in the x

                //* Try only z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //* Can move only in z
                    moveDir = moveDirZ;
                }
                else
                {
                    //* Cannot move any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        _isWalking = moveDir != Vector3.zero;
    }

    private float GetMovementSpeed()
    {
        if (HasGunObject())
        {
            if (GetGunObject().IsReload())
            {
                float moveReduct = 0.4f;
                return _moveSpeed - (_moveSpeed * moveReduct);
            }
        }
        return _moveSpeed;
    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this._selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCoutnerChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }

    //* Money
    public int GetPlayerMoney()
    {
        return this._playerMoney;
    }
    public void AddPlayerMoney(int money)
    {
        _playerMoney += money;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }

    //* Health
    public float GetPlayerHealth()
    {
        return this._playerHealth;
    }

    public void AddPlayerHealth(float health)
    {
        _playerHealth += health;
        if (_playerHealth > _playerSO.MaxHealth)
        {
            _playerHealth = _playerSO.MaxHealth;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    //* Armor
    public float GetPlayerArmor()
    {
        return this._playerArmor;
    }

    public void AddPlayerArmor(float armor)
    {
        _playerArmor += armor;
        if (_playerArmor > _playerSO.MaxArmor)
        {
            _playerArmor = _playerSO.MaxArmor;
        }
        OnArmorChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive) return;

        float damageResistance = 0.4f; //* 40%
        float damageReduce = damageResistance * damage;

        if (_playerArmor == 0)
        {
            _playerHealth -= damage;
        }
        else
        {
            if (_playerArmor > damageReduce)
            {
                float armorReduce = damage - damageReduce;
                _playerHealth -= damageReduce;
                _playerArmor -= armorReduce;
            }
            else
            {
                damageReduce = damage - _playerArmor;
                _playerHealth -= damageReduce;
                _playerArmor -= _playerArmor;
            }
        }

        if (_playerHealth <= 0)
        {
            _playerHealth = 0;
            Dead();
            OnDead?.Invoke(this, EventArgs.Empty);
        }
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnArmorChanged?.Invoke(this, EventArgs.Empty);

    }

    public bool IsAlive()
    {
        return this._isAlive;
    }

    public bool IsHoldShootAction()
    {
        return this._isHoldShootAction;
    }

    public bool IsWalking()
    {
        return this._isWalking;
    }

    public PlayerSO GetPlayerSO()
    {
        return this._playerSO;
    }

    public Transform GetGunObjectFollowTransform()
    {
        return this._gunObjectHoldPoint;
    }

    public void SetGunObject(GunObject gunObject)
    {
        this._gunObject = gunObject;
        if (HasGunObject())
        {
            OnPickGun?.Invoke(this, EventArgs.Empty);
            OnGunModeChanged?.Invoke(this, new OnGunModeChangedArgs
            {
                GunMode = GunObject.GunMode.Semi
            });
        }
    }

    public GunObject GetGunObject()
    {
        return this._gunObject;
    }

    public void ClearGunObject()
    {
        this._gunObject = null;
    }

    public bool HasGunObject()
    {
        return this._gunObject != null;
    }

    public Quaternion GetGunQuaternion()
    {
        return Quaternion.identity;
    }
}
