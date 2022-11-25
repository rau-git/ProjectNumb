using FishNet.Object;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterController : NetworkBehaviour
{
    [Header("Assign")] [SerializeField, Required, ChildGameObjectsOnly]
    private Rigidbody _playerRigidbody;

    [SerializeField, Required, ChildGameObjectsOnly]
    private GameObject _playerCamera;

    [SerializeField, Required, ChildGameObjectsOnly]
    private GameObject _groundCheckObject;

    private PlayerIngameControls _playerControls;

    [Header("Designer Options")] [SerializeField, SuffixLabel("m/s")]
    private float _movementForce;

    [SerializeField, SuffixLabel("m/s")] private float _maxMovementSpeed;

    [SerializeField, SuffixLabel("m/s²")] private float _movementAcceleration;

    [SerializeField, SuffixLabel("m/s")] private float _currentPlayerSpeed;

    [SerializeField] private float _jumpPower;

    [SerializeField] private int _mouseSensitivityMultiplier;

    [Header("Player Options")] [SerializeField]
    private float _mouseSensitivity;

    [Header("Hidden")] private float _xRotation = 0;
    private float _yRotation = 0;

    private Vector3 lastPosition;
    private Vector3 newPosition;

    private Vector2 _playerMovementInput;

    private bool _doJump;

    private void Awake()
    {
        _playerControls = new PlayerIngameControls();
        _playerControls.Enable();
    }

    private void Start()
    {
        CursorLockToggle(true);

        _mouseSensitivity *= _mouseSensitivityMultiplier;
        _doJump = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        _playerCamera.SetActive(IsOwner);
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        GetInput();
        MoveCamera();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        
        MovePlayer();
        Jump();
        Deaccelerate();
        BetterGravity();
    }

    private void GetInput()
    {
        _playerMovementInput = _playerControls.PlayerControls.Movement.ReadValue<Vector2>();
        _doJump = _playerControls.PlayerControls.Jump.WasPressedThisFrame();
    }

    private void MovePlayer()
    {
        var playerForwardBackMovement = Vector3.forward * _playerMovementInput.y;
        var playerLeftRightMovement = Vector3.right * _playerMovementInput.x;
        var playerMovementDirection = Vector3.ClampMagnitude(playerForwardBackMovement + playerLeftRightMovement, 1);

        if (_playerRigidbody.velocity.magnitude > _maxMovementSpeed)
        {
            return;
        }

        _playerRigidbody.AddRelativeForce(playerMovementDirection * _movementForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void Deaccelerate()
    {
        if (!_playerControls.PlayerControls.Movement.IsPressed())
        {
            _playerRigidbody.velocity = Vector3.Lerp(_playerRigidbody.velocity, Vector3.zero, _movementAcceleration);
        }
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

    private void Jump()
    {
        if (!_doJump || !CheckForGround()) return;
        
        _playerRigidbody.AddForce(transform.up * _jumpPower, ForceMode.VelocityChange);
    }

    private void StopMoving()
    {
        if (_playerRigidbody.velocity.magnitude < 0.1f)
        {
            _playerRigidbody.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    private void BetterGravity()
    {
        _playerRigidbody.AddForce(transform.up * Physics.gravity.y, ForceMode.Acceleration);
    }

    private bool CheckForGround()
    {
        RaycastHit hit;

        return Physics.Raycast(_groundCheckObject.transform.position, -transform.up, out hit, 0.1f);
    }

    private void CursorLockToggle(bool state)
    {
        if (state)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
