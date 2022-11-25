using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public sealed class PlayerController : NetworkBehaviour
{
    #region Assignables

    [Header("Assign")] [SerializeField] private Transform _cameraRoot;

    [SerializeField] private Transform _camera;

    private Rigidbody _playerRigidbody;

    private Animator _animator;

    [SerializeField] private LayerMask _groundCheck;

    private PlayerIngameControls _playerControls;

    #endregion

    #region DesignOptions

    [Header("Designer Options")] [SerializeField]
    private float _animationBlendSpeed = 8.9f;

    [SerializeField] private float _upperCameraLimit = -40f;

    [SerializeField] private float _bottomCameraLimit = 70f;

    [SerializeField] private float _distanceToGround = 0.8f;

    [SerializeField] private float _airResistance = 0.8f;

    private const float _walkSpeed = 2f;

    private const float _runSpeed = 6f;

    #endregion

    #region PlayerOptions

    [Header("Player Options")] [SerializeField]
    private float _mouseSensitivity;

    #endregion

    private int _xVelHash, _yVelHash, _jumpHash, _groundHash, _fallingHash, _zVelHash, _crouchHash;

    private bool _grounded;
    private bool _hasAnimator;
    private float _xRotation;
    private float _yRotation;
    private Vector2 _currentVelocity;

    public override void OnStartClient()
    {
        base.OnStartClient();

        _camera.GameObject().SetActive(IsOwner);

        if (!IsOwner) return;
        
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerControls = new PlayerIngameControls();
        IngameControlsOn(true);
        CursorLockState(true);
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        
        _hasAnimator = TryGetComponent(out _animator);
        AnimationAssignment();
    }

    private void AnimationAssignment()
    {
        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _zVelHash = Animator.StringToHash("Z_Velocity");
        _jumpHash = Animator.StringToHash("Jump");
        _groundHash = Animator.StringToHash("Grounded");
        _fallingHash = Animator.StringToHash("Falling");
        _crouchHash = Animator.StringToHash("Crouch");
    }

    private void IngameControlsOn(bool state)
    {
        switch (state)
        {
            case true:
            {
                _playerControls.Enable();
                break;
            }
            case false:
            {
                _playerControls.Disable();
                break;
            }
        }
    }

    private static void CursorLockState(bool state)
    {
        Cursor.visible = !state;

        Cursor.lockState = state switch
        {
            true => CursorLockMode.Locked,
            false => CursorLockMode.None
        };
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        SampleGround();
        Move();
        HandleJump();
        HandleCrouch();
    }

    private void Update()
    {
        if (!IsOwner) return;

        CamMovements();
    }

    private void Move()
        {
            if(!_hasAnimator) return;

            var targetSpeed = _playerControls.PlayerControls.Run.IsPressed() ? _runSpeed : _walkSpeed;

            if(_playerControls.PlayerControls.Crouch.IsPressed()) targetSpeed = _walkSpeed * 0.5f;
            
            if(_playerControls.PlayerControls.Movement.ReadValue<Vector2>() == Vector2.zero) targetSpeed = 0;

            if(_grounded)
            {
                
                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _playerControls.PlayerControls.Movement.ReadValue<Vector2>().x * targetSpeed, _animationBlendSpeed * Time.deltaTime);
                _currentVelocity.y =  Mathf.Lerp(_currentVelocity.y, _playerControls.PlayerControls.Movement.ReadValue<Vector2>().y * targetSpeed, _animationBlendSpeed * Time.deltaTime);

                var velocity = _playerRigidbody.velocity;
                var xVelDifference = _currentVelocity.x - velocity.x;
                var zVelDifference = _currentVelocity.y - velocity.z;

                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0 , zVelDifference)), ForceMode.VelocityChange);
            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * _airResistance,0,_currentVelocity.y * _airResistance)), ForceMode.VelocityChange);
            }


            _animator.SetFloat(_xVelHash , _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }

        private void CamMovements()
        {
            if(!_hasAnimator) return;

            var looking = _playerControls.PlayerControls.Look.ReadValue<Vector2>();

            var mouseX = looking.x * _mouseSensitivity * Time.deltaTime;
            var mouseY = looking.y * _mouseSensitivity * Time.deltaTime;
            
            _camera.position = _cameraRoot.position;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, _upperCameraLimit, _bottomCameraLimit);
            
            _yRotation += mouseX;

            _camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0);
            transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        }

        private void HandleCrouch()
        {
            if (!_hasAnimator) return;
            
            _animator.SetBool(_crouchHash, _playerControls.PlayerControls.Crouch.IsPressed());
        }


        private void HandleJump()
        {
            if(!_hasAnimator) return;
            
            if(!_playerControls.PlayerControls.Jump.IsPressed()) return;
            
            if(!_grounded) return;
            
            _animator.SetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if(!_hasAnimator) return;

            if(Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out _, _distanceToGround + 0.1f, _groundCheck))
            {
                _grounded = true;
                SetAnimationGrounding();
                return;
            }
            _grounded = false;
            _animator.SetFloat(_zVelHash, _playerRigidbody.velocity.y);
            SetAnimationGrounding();
        }

        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
    }
