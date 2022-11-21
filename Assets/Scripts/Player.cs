using System;
using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerIngameControls _playerIngameControls;
    private Rigidbody _playerRigidbody;
    [SerializeField] private GameObject _groundCheckPoint;
    [SerializeField] private float _playerMoveSpeed;
    [SerializeField] private float _playerJumpPower;
    private float _gravity;

    private void Awake()
    {
        _playerIngameControls = new PlayerIngameControls();
        _playerIngameControls.Enable();
        _playerRigidbody = GetComponent<Rigidbody>();
        _gravity = Physics.gravity.y;
    }

    private void OnEnable()
    {
        _playerIngameControls.Enable();
    }

    private void OnDisable()
    {
        _playerIngameControls.Disable();
    }

    private void Update()
    {
        MovePlayer();
        Jump();
    }

    private void MovePlayer()
    {
        var playerMovementInput = _playerIngameControls.PlayerControls.Movement.ReadValue<Vector2>();
        var playerMovementDirection = new Vector3(playerMovementInput.x, 0, playerMovementInput.y);

        _playerRigidbody.MovePosition(transform.position + playerMovementDirection * _playerMoveSpeed * Time.deltaTime);
    }
    
    private void Jump()
    {
        if (_playerIngameControls.PlayerControls.Jump.WasPressedThisFrame())
        {
            _playerRigidbody.AddForce(transform.up * _playerJumpPower, ForceMode.VelocityChange);
        }
    }

    private void BetterGravity()
    {
        if (!CheckForGround()) return;
        
        _playerRigidbody.AddForce(transform.up * _gravity, ForceMode.Acceleration);
    }

    private bool CheckForGround()
    {
        RaycastHit hit;

        return Physics.Raycast(_groundCheckPoint.transform.position, -transform.up, out hit, 0.1f);
    }
}
