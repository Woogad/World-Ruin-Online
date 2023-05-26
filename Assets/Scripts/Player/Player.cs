using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using IngameDebugConsole;

public class Player : NetworkBehaviour, IGunObjectParent, IDamageable
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPlayerPickGun;

    public static Player LocalInstance { get; private set; }

    public static void ResetStaticEvent()
    {
        OnAnyPlayerSpawned = null;
    }

    public event EventHandler OnInteract;
    public event EventHandler OnAmmoChanged;
    public event EventHandler OnMoneyChanged;
    public event EventHandler OnRelaod;
    public event EventHandler OnShoot;
    public event EventHandler OnArmorChanged;
    public event EventHandler OnHealthChanged;
    public event EventHandler OnPickGun;
    public event EventHandler OnTakeDamage;
    public event EventHandler<OnDeadArgs> OnDead;
    public class OnDeadArgs : EventArgs
    {
        public ulong ClientID;
    }

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
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _gunObjectHoldPoint;

    private NetworkVariable<float> _playerHealth = new NetworkVariable<float>();
    private NetworkVariable<float> _playerArmor = new NetworkVariable<float>();
    private NetworkVariable<int> _playerMoney = new NetworkVariable<int>();
    private NetworkVariable<bool> _isAlive = new NetworkVariable<bool>(true);
    private NetworkVariable<int> _killScore = new NetworkVariable<int>();

    private float defaultHealth = 60f;
    private float defaultAromr = 0f;
    private bool defaultIsAlive = true;
    [SerializeField] private int defaulMoney = 5000; //! SerializeField for testing

    private bool _isHoldShootAction;
    private bool _isWalking;
    private float _reloadCountdown;
    private BaseCounter _selectedCounter;
    private GunObject _gunObject;
    private ulong _playerKilled;

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
        GameInput.Instance.OnShootWeaponHoldAction += GameInputOnShootAutoAction;
        GameInput.Instance.OnShootWeaponAction += GameInputOnShootAction;
        GameInput.Instance.OnReloadAction += GameInputOnReloadAction;
        GameInput.Instance.OnToggleWeaponModeAction += GameInputOnToggleWeaponModeAction;
        DebugLogConsole.AddCommand<float, ulong>("TakeDamage", "Get Damage", TakeDamage);
    }

    public override void OnNetworkSpawn()
    {
        PlayerSetup(defaultHealth, defaultAromr, defaulMoney, defaultIsAlive);

        _playerHealth.OnValueChanged += PlayerHealthValueChanged;
        _playerArmor.OnValueChanged += PlayerArmorValueChanged;
        _playerMoney.OnValueChanged += PlayerMoneyValueChanged;
        _isAlive.OnValueChanged += PlayerIsAliveValueChanged;
        _killScore.OnValueChanged += PlayerKillScoreValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManagerOnClientDisconnectCallback;
        }
        if (IsOwner)
        {
            LocalInstance = this;
            // FollowPlayerCamera.Instance.CameraFollow(gameObject.transform);
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerKillScoreValueChanged(int previousValue, int newValue)
    {
        Debug.Log("Player " + OwnerClientId + " Scorekill " + _killScore.Value);
    }

    private void NetworkManagerOnClientDisconnectCallback(ulong clientID)
    {
        if (clientID == OwnerClientId && HasGunObject())
        {
            GunObject.DestroyGunObject(GetGunObject());
        }
    }

    private void PlayerIsAliveValueChanged(bool previousValue, bool newValue)
    {
        OnDead?.Invoke(this, new OnDeadArgs
        {
            ClientID = _playerKilled
        });
    }

    private void PlayerArmorValueChanged(float previousValue, float newValue)
    {
        OnArmorChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerMoneyValueChanged(int previousValue, int newValue)
    {
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerHealthValueChanged(float previousValue, float newValue)
    {
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (HasGunObject())
        {
            if (GetGunObject().IsReload())
            {
                _reloadCountdown += Time.deltaTime;
                OnReloadProgressChanged?.Invoke(this, new OnReloadProgressChangedArgs
                {
                    ReloadProgressNormalized = _reloadCountdown / GetGunObject().GetGunObjectSO().ReloadTime
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
                GetGunObject().Shoot(OwnerClientId);

                OnShoot?.Invoke(this, EventArgs.Empty);
                OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        HandleMovement();

        PlayerLookAtMouse();

        HandleInteraction();
    }

    private void GameInputOnShootAction(object sender, EventArgs e)
    {
        if (!IsOwner) return;
        if (HasGunObject())
        {
            if (GetGunObject().GetGunMode() != GunObject.GunMode.Semi) return;
            if (GetGunObject().TryShoot())
            {
                GetGunObject().Shoot(OwnerClientId);
                OnShoot?.Invoke(this, EventArgs.Empty);
                OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    private void GameInputOnToggleWeaponModeAction(object sender, EventArgs e)
    {
        if (!IsOwner) return;
        if (HasGunObject())
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
        if (!IsOwner) return;
        if (HasGunObject())
        {
            if (CanReload())
            {
                StartCoroutine(GetGunObject().ReloadTimeCoroutine());

                OnRelaod?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameInputOnShootAutoAction(object sender, GameInput.OnShootWeaponActionArgs e)
    {
        if (!IsOwner) return;
        _isHoldShootAction = e.IsHoldShootAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            if (HasGunObject())
            {
                if (!GetGunObject().IsReload())
                {
                    _selectedCounter.Interact(this);
                    OnInteract?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                _selectedCounter.Interact(this);
                OnInteract?.Invoke(this, EventArgs.Empty);
            }
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

    private void PlayerSetup(float health, float armor, int money, bool isAlive)
    {
        PlayerSetupServerRpc(health, armor, money, isAlive);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerSetupServerRpc(float health, float armor, int money, bool isAvlive)
    {
        if (health > _playerSO.MaxHealth || armor > _playerSO.MaxArmor || money > _playerSO.MaxMoney)
        {
            Debug.LogError("PlayerSetup is out of limit!");
        }
        this._playerHealth.Value = health;
        this._playerArmor.Value = armor;
        this._playerMoney.Value = money;
        this._isAlive.Value = isAvlive;
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

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

    private void PlayerLookAtMouse()
    {
        if (!_isAlive.Value) return;
        //* Make Player lookAt mouse
        transform.LookAt(Mouse3D.Instance.GetAngleAimRotateTransform());
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
        return this._playerMoney.Value;
    }
    public void AddPlayerMoney(int money)
    {
        AddPlayerMoneyServerRpc(money);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerMoneyServerRpc(int money)
    {
        _playerMoney.Value += money;
    }

    //* Health
    public float GetPlayerHealth()
    {
        return this._playerHealth.Value;
    }

    public void AddPlayerHealth(float health)
    {
        AddPlayerHealthServerRpc(health);
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerHealthServerRpc(float health)
    {
        _playerHealth.Value += health;
        if (_playerHealth.Value > _playerSO.MaxHealth)
        {
            _playerHealth.Value = _playerSO.MaxHealth;
        }
    }

    //* Armor
    public float GetPlayerArmor()
    {
        return this._playerArmor.Value;
    }

    public void AddPlayerArmor(float armor)
    {
        AddPlayerArmorServerRpc(armor);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerArmorServerRpc(float armor)
    {

        _playerArmor.Value += armor;
        if (_playerArmor.Value > _playerSO.MaxArmor)
        {
            _playerArmor.Value = _playerSO.MaxArmor;
        }
    }

    public void TakeDamage(float damage, ulong shootOwnerClientID)
    {
        // if (!_isAlive.Value) return;
        TakeDamageServerRpc(damage, shootOwnerClientID);
        OnTakeDamage?.Invoke(this, EventArgs.Empty);

    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(float damage, ulong shootOwnerClientID)
    {
        if (!_isAlive.Value) return;
        float damageResistance = 0.4f; //* 40%
        float damageReduce = damageResistance * damage;

        if (_playerArmor.Value == 0)
        {
            _playerHealth.Value -= damage;
        }
        else
        {
            if (_playerArmor.Value > damageReduce)
            {
                float armorReduce = damage - damageReduce;
                _playerHealth.Value -= damageReduce;
                _playerArmor.Value -= armorReduce;
            }
            else
            {
                damageReduce = damage - _playerArmor.Value;
                _playerHealth.Value -= damageReduce;
                _playerArmor.Value -= _playerArmor.Value;
            }
        }

        if (_playerHealth.Value <= 0)
        {
            Dead(shootOwnerClientID);
            _isAlive.Value = false;
            _playerHealth.Value = 0;
        }
    }

    private void Dead(ulong clientID)
    {
        DeadClientRpc();
        _playerKilled = clientID;
    }

    [ClientRpc]
    private void DeadClientRpc()
    {
        Collider playerCollider = GetComponent<Collider>();
        playerCollider.enabled = false;
        _isWalking = false;
        OnReloadProgressChanged?.Invoke(this, new OnReloadProgressChangedArgs
        {
            ReloadProgressNormalized = 0
        });
        // GameMultiplayer.Instance.DestroyGunObject(GetGunObject());
    }

    public void AddKillScoreNetworkVariable(int score)
    {
        Debug.Log("player " + OwnerClientId + " use Add kill score");
        _killScore.Value += score;
    }

    public int GetKillScore()
    {
        return _killScore.Value;
    }

    public bool IsAlive()
    {
        return this._isAlive.Value;
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
            OnAnyPlayerPickGun?.Invoke(this, EventArgs.Empty);
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

    public Vector3 GetLocalScale()
    {
        return Vector3.one;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
