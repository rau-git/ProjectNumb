using UnityEngine;
using FishNet.Object;

public class FishNetworkingMovement : NetworkBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    private Rigidbody _rigidbody;
    [SerializeField]
    private float _maxSpeed;

    private PlayerIngameControls _playerControls;

    [SerializeField] 
    private GameObject _playerCamera;

    private Vector2 _playerMovementInput;

    [SerializeField] 
    private float _mouseSensitivity;

    private float _xRotation;
    private float _yRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerControls = new PlayerIngameControls();
        _playerControls.Enable();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        _playerCamera.GetComponent<Camera>().enabled = IsOwner;
        _playerCamera.GetComponent<AudioListener>().enabled = IsOwner;
    }

    private void Update()
    {
        if (!IsOwner) return;

        GetInput();
        MoveCamera();
    }

    private void GetInput()
    {
        _playerMovementInput = _playerControls.PlayerControls.Movement.ReadValue<Vector2>();
        
        MovePlayer(_playerMovementInput);
    }

    private void MovePlayer(Vector2 movement)
    {
        var playerForwardBackMovement = Vector3.forward * _playerMovementInput.y;
        var playerLeftRightMovement = Vector3.right * _playerMovementInput.x;
        var playerMovementDirection = Vector3.ClampMagnitude(playerForwardBackMovement + playerLeftRightMovement, 1);
        
        if(_rigidbody.velocity.magnitude > _maxSpeed) return;
        
        _rigidbody.AddForce(playerMovementDirection * _moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
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
        transform.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
