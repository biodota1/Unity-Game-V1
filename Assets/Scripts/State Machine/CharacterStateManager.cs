using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class CharacterStateManager : MonoBehaviour
{
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;

    int _isWalkingHash;
    int _isRunningHash;
    int _isEscapeHash;
    int _jumpCountHash;
    int _isCastingHash;

    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;
    bool _isEscapePressed;
    bool _isCastPressed;

    float _rotationFactorPerFrame = 15.0f;
    int _zero = 0;
    float _runMultiplier = 5.0f;

    float _gravity = -9.81f;
    float _groundedGravity = -0.05f;

    float _initalJumpVelocity;
    float _maxJumpHeight = 1.0f;
    float _maxJumpTime = 0.75f;
    bool _requireNewJumpPress = false;
    int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _dashCoroutine = null;


    CharacterBaseState _currentState;
    CharacterStateFactory _states;
    

    public CharacterBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController Controller { get { return _characterController; } }
    public Animator Animator { get { return _animator; } }
    public Coroutine DashCoroutine { get { return _dashCoroutine; } set { _dashCoroutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public int JumpCount { get { return _jumpCount; }  set { _jumpCount = value; } }
    public int IsEscapeHash { get { return _isEscapeHash; } }
    public int IsWalkingHash { get { return _isWalkingHash; } }
    public int IsRunningHash { get { return _isRunningHash; } }
    public int JumpCountHash { get { return _jumpCountHash; } }
    public int IsCastingHash { get { return _isCastingHash; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public bool IsEscapePressed { get { return _isEscapePressed; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsCastPressed { get { return _isCastPressed; } }
    public float GroundedGravity { get { return _groundedGravity; }}
    public float RunMultiplier { get { return _runMultiplier; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
    public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } set { _currentMovementInput = value; } } 



    void Awake()
    {
       _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _states = new CharacterStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isEscapeHash = Animator.StringToHash("isEscaping");
        _jumpCountHash = Animator.StringToHash("isjumpCount");
        _isCastingHash = Animator.StringToHash("isCasting");


        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;
        _playerInput.CharacterControls.Escape.started += OnEscape;
        _playerInput.CharacterControls.Escape.canceled += OnEscape;
        _playerInput.CharacterControls.Cast.started += OnCast;
        _playerInput.CharacterControls.Cast.canceled += OnCast;

        SetupJumpVariables();
    }

  

    void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initalJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (_maxJumpHeight * 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (_maxJumpHeight * 2)) / timeToApex * 1.25f;
        float thirdJumpGravity = (-2 * (_maxJumpHeight * 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (_maxJumpHeight * 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, _initalJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        _jumpGravities.Add(0, _gravity);
        _jumpGravities.Add(1, _gravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }


    void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovementInput.x;
        positionToLookAt.y = _zero;
        positionToLookAt.z = _currentMovementInput.y;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }

    }

    void Update()
    {
        HandleRotation();
        _currentState.UpdateStates();
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    private void OnCast(InputAction.CallbackContext context)
    {
        _isCastPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }

    void OnEscape(InputAction.CallbackContext context)
    {
        _isEscapePressed = context.ReadValueAsButton();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}

