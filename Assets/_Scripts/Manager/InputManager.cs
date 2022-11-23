using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 Move {get; private set;}
        public bool Run {get; private set;}
        public bool Jump {get; private set;}
        public bool Crouch {get; private set;}

        private InputActionMap _currentMap;
        private InputAction _moveAction;
        private InputAction _runAction;
        private InputAction _jumpAction;
        private InputAction _crouchAction;

        private void Awake() {
            _currentMap = PlayerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _runAction  = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump");
            _crouchAction = _currentMap.FindAction("Crouch");

            _moveAction.performed += onMove;
            _runAction.performed += onRun;
            _jumpAction.performed += onJump;
            _crouchAction.started += onCrouch;

            _moveAction.canceled += onMove;
            _runAction.canceled += onRun;
            _jumpAction.canceled += onJump;
            _crouchAction.canceled += onCrouch;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        
        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        private void onJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }
        private void onCrouch(InputAction.CallbackContext context)
        {
            Crouch = context.ReadValueAsButton();
        }

        private void OnEnable() {
            _currentMap.Enable();
        }

        private void OnDisable() {
            _currentMap.Disable();
        }
        
    }
}
