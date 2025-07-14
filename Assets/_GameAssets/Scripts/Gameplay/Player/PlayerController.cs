using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;

    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private KeyCode _movementKey;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _airDrag;
    [SerializeField] private bool _canJump;

    [Header("Slider Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;
    [SerializeField] private bool _isSliding;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;
    private StateControl _stateControl;
    private Rigidbody _playerRigidbody;

    private float _startingMovementSpeed, _startinJumpForce;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private void Awake()
    {
        _stateControl = GetComponent<StateControl>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        _startingMovementSpeed = _movementSpeed;
        _startinJumpForce = _jumpForce;
    }
    private void Update()
    {
        SetInputs();
        SetStates();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }
    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }

        else if (Input.GetKeyDown(_movementKey))

        {
            _isSliding = false;

        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }
    private void SetStates()
    {
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var CurrentState = _stateControl.GetCurrentState();

        var newState = CurrentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !_isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !_isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && _isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && _isSliding => PlayerState.SlideIdle,
            _ when !_canJump && isGrounded => PlayerState.Jump,
            _ => CurrentState
        };

        if (newState != CurrentState)
        {
            _stateControl.ChangeState(newState);
        }
    }
    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput +
                _orientationTransform.right * _horizontalInput;

        float forceMultiplier = _stateControl.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Idle => _slideMultiplier,
            PlayerState.Jump => _airMultiplier,
            _ => 1f
        };
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplier, ForceMode.Force);
    }
    private void SetPlayerDrag()
    {
        _playerRigidbody.linearDamping = _stateControl.GetCurrentState() switch
        {
            PlayerState.Move => _groundDrag,
            PlayerState.Slide => _slideDrag,
            PlayerState.Jump => _airDrag,
            _ => _playerRigidbody.linearDamping
        };
    }
    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity
        = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);

        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y
            , limitedVelocity.z);
        }
    }
    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJumping()
    {
        _canJump = true;
    }

    #region Helper Functions
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }
    private Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;
    }
    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }
    private void ResetMovementSpeed()
    {
        _movementSpeed = _startingMovementSpeed;
    }

    public void SetJumpForce(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }
    private void ResetJumpForce()
    {
        _jumpForce = _startinJumpForce;
    }
    #endregion
}

 