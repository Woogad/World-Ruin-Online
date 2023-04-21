using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGunObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnShootArgs> OnShoot;
    public class OnShootArgs : EventArgs
    {
        //TODO Add OnshootArgs 
    }
    public event EventHandler OnInteract;
    public event EventHandler<OnSelectedCoutnerChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCoutnerChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotaionSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _gunObjectHoldPoint;
    [SerializeField] private Mouse3D _mouse3D;
    [SerializeField] private int _playerMoney;
    [SerializeField] private Transform _prefabBullet;

    private bool _isWalking;
    private BaseCounter _selectedCounter;
    private GunObject _gunObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInputOnInteractAction;
        _gameInput.OnShootWeaponAction += GameInputOnShootAction;
    }


    private void Update()
    {
        HandleMovement();

        //* Make Player lookAt mouse
        transform.LookAt(_mouse3D.GetAngleAimRotate());

        HandleInteraction();
    }


    public bool IsWalking()
    {
        return _isWalking;
    }

    private void GameInputOnShootAction(object sender, EventArgs e)
    {
        if (HasGunObject())
        {
            Transform fireEndPoint = GetGunObject().GetFireEndPointTransform();
            ShootConfigOS shootConfigOS = GetGunObject().GetShootConfigOS();

            Transform bulletTransform = Instantiate(_prefabBullet, fireEndPoint.position, Quaternion.identity);
            bulletTransform.GetComponent<BulletObject>().Setup(fireEndPoint, shootConfigOS.Damage);
        }
    }

    private void GameInputOnInteractAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
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
        return _playerMoney;
    }
    public void SetPlayerMoney(int money)
    {
        _playerMoney = money;
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
