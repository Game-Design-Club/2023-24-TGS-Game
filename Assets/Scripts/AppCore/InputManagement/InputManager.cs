using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public class InputManager : MonoBehaviour {
        public event Action<Vector2> OnMovementInput;
        public event Action OnInteractPressed;
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnClickWorld;
        
        private InputActions _inputActions;
        
        private void Awake() {
            _inputActions = new InputActions();
        }
        
        private void OnEnable() {
            _inputActions.Enable();
            EnableMovement();
            EnableInteract();
            EnableClicking();
        }

        private void OnDisable() {
            _inputActions.Disable();
            DisableMovement();
            DisableInteract();
            DisableClicking();
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
        
        private void EnableClicking() {
            _inputActions.UI.Click.Enable();
            _inputActions.UI.Click.performed += OnClickPerformed;
        }
        
        private void DisableClicking() {
            _inputActions.UI.Click.Disable();
            _inputActions.UI.Click.performed -= OnClickPerformed;
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            Vector2 movementInput = context.ReadValue<Vector2>();
            OnMovementInput?.Invoke(movementInput);
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext context) {
            OnInteractPressed?.Invoke();
        }
        
        private void OnClickPerformed(InputAction.CallbackContext context) {
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            Vector2 clickPosition = Mouse.current.position.ReadValue();
            OnClick?.Invoke(clickPosition);
            OnClickWorld?.Invoke(Camera.main.ScreenToWorldPoint(clickPosition));
        }
    }
}