using UnityEngine;
using FishNet.Object;

public class FishNetworkingMovement : NetworkBehaviour
{
    [Header("Assign Objects")] 
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _HUD;
    private Rigidbody _rigidbody;
    private PlayerIngameControls _playerControls;
    
    [Header("Movement Variables")]
    [SerializeField] private float _moveForce;
    [SerializeField] private float _airForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _deaccelerationSpeed;

    [Header("Player Options")]
    [SerializeField] private float _mouseSensitivity;

    #region Class Variables
    private float _xRotation;
    private float _yRotation;
    private Vector2 _playerMovementInput;
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerControls = new PlayerIngameControls();
        _playerControls.Enable();
        LockCursor(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // We have to disable components other players would end up also using
        _playerCamera.GetComponent<Camera>().enabled = IsOwner;
        _playerCamera.GetComponent<AudioListener>().enabled = IsOwner;
        _HUD.SetActive(IsOwner);
    }

    private void Update()
    {
        if (!IsOwner) return;

        GetInput();
        MovePlayer();
        MoveCamera();
    }

    private void GetInput()
    {
        _playerMovementInput = _playerControls.PlayerControls.Movement.ReadValue<Vector2>();

        if (_playerMovementInput == Vector2.zero)
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, _deaccelerationSpeed);
        }
    }

    private void MovePlayer()
    {
        var currentMoveForce = _moveForce;

        var playerForwardBackMovement = transform.forward * _playerMovementInput.y;
        var playerLeftRightMovement = transform.right * _playerMovementInput.x;
        var playerMovementDirection = Vector3.ClampMagnitude(playerForwardBackMovement + playerLeftRightMovement, 1);
        
        if (CheckForGround()) currentMoveForce = _airForce;
        
        if(_rigidbody.velocity.magnitude > _maxSpeed) return;
        
        _rigidbody.AddForce(playerMovementDirection * currentMoveForce * Time.deltaTime, ForceMode.Force);
    }
    
    private void MoveCamera()
    {
        var looking = _playerControls.PlayerControls.Look.ReadValue<Vector2>();
        
        var mouseX = looking.x * _mouseSensitivity * Time.deltaTime;
        var mouseY = looking.y * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _yRotation += mouseX;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        gameObject.transform.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    private bool CheckForGround()
    {
        Physics.Raycast(transform.position, -Vector3.up, out var hit, 0.1f);
        
        return hit.collider != null;
    }

    private static void LockCursor(bool state)
    {
        switch (state)
        {
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            
            case false:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
}
