using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public class InputManager : MonoBehaviour {
        public event Action<Vector2> OnMovementInput;
        public event Action OnInteractPressed;
        public event Action OnCancelPressed;
        
        private InputActions _inputActions;
        
        private void Awake() {
            _inputActions = new InputActions();
        }
        
        private void OnEnable() {
            _inputActions.Enable();
            EnableMovement();
            EnableInteract();
            EnableCancel();
        }

        private void OnDisable() {
            _inputActions.Disable();
            DisableMovement();
            DisableInteract();
            DisableCancel();
        }


        private void EnableMovement() {
            _inputActions.Player.Move.Enable();
            _inputActions.Player.Move.performed += OnMovementPerformed;
            _inputActions.Player.Move.canceled += OnMovementPerformed;
        }
        
        private void DisableMovement() {
            _inputActions.Player.Move.Disable();
            _inputActions.Player.Move.performed -= OnMovementPerformed;
            _inputActions.Player.Move.canceled -= OnMovementPerformed;
        }
        
        private void EnableInteract() {
            _inputActions.UI.Interact.Enable();
            _inputActions.UI.Interact.performed += OnInteractPerformed;
        }
        
        private void DisableInteract() {
            _inputActions.UI.Interact.Disable();
            _inputActions.UI.Interact.performed -= OnInteractPerformed;
        }
        
        private void EnableCancel() {
            _inputActions.UI.Cancel.Enable();
            _inputActions.UI.Cancel.performed += OnCancelPerformed;
        }
        
        private void DisableCancel() {
            _inputActions.UI.Cancel.Disable();
            _inputActions.UI.Cancel.performed -= OnCancelPerformed;
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            Vector2 movementInput = context.ReadValue<Vector2>();
            OnMovementInput?.Invoke(movementInput);
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext context) {
            OnInteractPressed?.Invoke();
        }
        
        private void OnCancelPerformed(InputAction.CallbackContext context) {
            OnCancelPressed?.Invoke();
        }
    }
}