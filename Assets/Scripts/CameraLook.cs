using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class CameraLook : MonoBehaviour
{
    [SerializeField] 
    private float _mouseSensitivity;
    
    [SerializeField] 
    private Rigidbody _playerBody;
    
    private float _xRotation = 0;
    private float _yRotation = 0;

    private PlayerIngameControls _playerIngameControls;

    private void Awake()
    {
        _playerIngameControls = new PlayerIngameControls();
        _playerIngameControls.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _mouseSensitivity *= 10;
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        var looking = _playerIngameControls.PlayerControls.Look.ReadValue<Vector2>();
        var mouseX = looking.x * _mouseSensitivity * Time.deltaTime;
        var mouseY = looking.y * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _yRotation += mouseX;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.transform.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
