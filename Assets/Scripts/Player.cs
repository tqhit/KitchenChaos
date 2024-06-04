using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter { get; private set; }

        public OnSelectedCounterChangedEventArgs(ClearCounter selectedCounter)
        {
            SelectedCounter = selectedCounter;
        }
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    
    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private ClearCounter _selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance in the scene!");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter != null)
            _selectedCounter.Interact();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
            _lastInteractDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance, _countersLayerMask)) 
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
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

        Vector3 moveDir = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Can not move in any direction
                }
            }
        }

        if (canMove)
            transform.position += moveDistance * moveDir;

        _isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * _rotationSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs(selectedCounter));
    }
}
