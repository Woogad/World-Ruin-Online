using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGunObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnInteract;
    public event EventHandler OnUpdateAmmo;
    public event EventHandler OnUpdateMoney;
    public event EventHandler OnAddArmor;
    public event EventHandler OnAddHealth;
    public event EventHandler<OnSelectedCoutnerChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCoutnerChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private PlayerSO _playerSO;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotaionSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _gunObjectHoldPoint;
    [SerializeField] private Mouse3D _mouse3D;

    private float _playerHealth;
    private float _playerArmor;
    private int _playerMoney;

    private bool _isWalking;
    private BaseCounter _selectedCounter;
    private GunObject _gunObject;
    private bool _isHoldShootAction;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance!");
        }
        Instance = this;

        float defaultHealth = 50f;
        float defaultAromr = 5f;
        int defaulMoney = 500;
        PlayerSetup(defaultHealth, defaultAromr, defaulMoney);
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInputOnInteractAction;
        _gameInput.OnShootWeaponHoldAction += GameInputOnShootHoldAction;
        _gameInput.OnShootWeaponAction += GameInputOnShootAction;
        _gameInput.OnReloadAction += GameInputOnReloadAction;
        _gameInput.OnToggleWeaponModeAction += GameInputOnToggleWeaponModeAction;
    }

    private void GameInputOnShootAction(object sender, EventArgs e)
    {
        if (HasGunObject())
        {
            if (GetGunObject().GetGunMode() != GunObject.GunMode.Semi) return;

            GetGunObject().Shoot();
            OnUpdateAmmo?.Invoke(this, EventArgs.Empty);

        }
    }

    private void GameInputOnToggleWeaponModeAction(object sender, EventArgs e)
    {
        GetGunObject().CycleGunMode();
    }

    private void Update()
    {
        if (HasGunObject())
        {
            if (CanShootAuto())
            {
                GetGunObject().Shoot();
                OnUpdateAmmo?.Invoke(this, EventArgs.Empty);
            }
        }

        HandleMovement();

        //* Make Player lookAt mouse
        transform.LookAt(_mouse3D.GetAngleAimRotate());

        HandleInteraction();
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


    private void GameInputOnReloadAction(object sender, EventArgs e)
    {
        if (HasGunObject())
        {
            GetGunObject().Reload();
            OnUpdateAmmo?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInputOnShootHoldAction(object sender, GameInput.OnShootWeaponActionArgs e)
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
        if (_isHoldShootAction && GetGunObject().TryShoot()) return true;
        return false;
    }


    private void HandleInteraction()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

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

        float moveDistance = _moveSpeed * Time.deltaTime;
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
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotaionSpeed);

    }

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this._selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCoutnerChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }

    public int GetPlayerMoney()
    {
        return this._playerMoney;
    }
    public void AddPlayerMoney(int money)
    {
        _playerMoney += money;
        OnUpdateMoney?.Invoke(this, EventArgs.Empty);
    }

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
        OnAddHealth?.Invoke(this, EventArgs.Empty);
    }

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
        OnAddArmor?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(float damage)
    {
        float damageReduce = 0.4f; //* 40%

        float armorReduce = damageReduce * damage;
        if (_playerArmor == 0)
        {
            _playerHealth -= damage;
            return;
        }
        if (_playerArmor > armorReduce)
        {
            float hpReduce = damage - armorReduce;
            _playerHealth -= hpReduce;
            _playerArmor -= armorReduce;
        }
        else
        {
            float hpReduce = damage - _playerArmor;
            _playerHealth -= hpReduce;
            _playerArmor -= _playerArmor;
        }

    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    public PlayerSO GetPlayerSO()
    {
        return this._playerSO;
    }

    public Transform GetGunObjectFollowTransform()
    {
        return _gunObjectHoldPoint;
    }

    public void SetGunObject(GunObject gunObject)
    {
        this._gunObject = gunObject;
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
        return _gunObject != null;
    }

    public Quaternion GetGunQuaternion()
    {
        return Quaternion.identity;
    }
}
