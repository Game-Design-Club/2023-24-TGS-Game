using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public class InputManager : MonoBehaviour {
        public event Action<Vector2> OnMovementInput;
        
        private InputActions _inputActions;
        
        private void Awake() {
            _inputActions = new InputActions();
        }
        
        private void OnEnable() {
            _inputActions.Enable();
            EnableMovement();
        }

        private void OnDisable() {
            _inputActions.Disable();
            DisableMovement();
        }


        private void EnableMovement() {
            _inputActions.Player.Movement.Enable();
            _inputActions.Player.Movement.performed += OnMovementPerformed;
            _inputActions.Player.Movement.canceled += OnMovementPerformed;
        }
        
        private void DisableMovement() {
            _inputActions.Player.Movement.Disable();
            _inputActions.Player.Movement.performed -= OnMovementPerformed;
            _inputActions.Player.Movement.canceled -= OnMovementPerformed;
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            Vector2 movementInput = context.ReadValue<Vector2>();
            OnMovementInput?.Invoke(movementInput);
        }
    }
}