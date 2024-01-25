using System;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement {
    public class InputManager : MonoBehaviour {
        private InputActions _inputActions;
        private bool _lockedControls;
        private Vector2 _lastMovementInput;
        
        // UI
        public event Action OnCancel;
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnClickWorld;
        public event Action OnPoint;
        
        // Player
        public event Action<Vector2> OnMovement;
        public event Action OnInteract;
        public event Action OnInteractCancel;
        
        private void Awake() {
            _inputActions = new InputActions();
        }
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
            _inputActions.Enable();
            EnableAll();
        }
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
            _inputActions.Disable();
            DisableAll();
        }
        
        private void EnableAll() {
            EnableMovement();
            EnableInteract();
            EnableCancel();
            EnableClicking();
            EnableMouseMovement();
            void EnableMovement() {
                _inputActions.Player.Move.Enable();
                _inputActions.Player.Move.performed += OnMovementPerformed;
                _inputActions.Player.Move.canceled += OnMovementPerformed;
            }
            void EnableInteract() {
                _inputActions.Player.Interact.Enable();
                _inputActions.Player.Interact.performed += OnInteractPerformed;
                _inputActions.Player.Interact.canceled += OnInteractCancelled;
            }
            void EnableCancel() {
                _inputActions.UI.Cancel.Enable();
                _inputActions.UI.Cancel.performed += OnCancelPerformed;
            }
            void EnableClicking() {
                _inputActions.UI.Click.Enable();
                _inputActions.UI.Click.performed += OnClickPerformed;
            }
            void EnableMouseMovement() {
                _inputActions.UI.Point.Enable();
                _inputActions.UI.Point.performed += OnPointPerformed;
            }
        }
        private void DisableAll() {
            DisableMovement();
            DisableInteract();
            DisableCancel();
            DisableClicking();
            void DisableMovement() {
                _inputActions.Player.Move.Enable();
                _inputActions.Player.Move.performed -= OnMovementPerformed;
                _inputActions.Player.Move.canceled -= OnMovementPerformed;
            }
            void DisableInteract() {
                _inputActions.Player.Interact.Disable();
                _inputActions.Player.Interact.performed -= OnInteractPerformed;
                _inputActions.Player.Interact.canceled -= OnInteractCancelled;
            }
            void DisableCancel() {
                _inputActions.UI.Cancel.Disable();
                _inputActions.UI.Cancel.performed -= OnCancelPerformed;
            }
            void DisableClicking() {
                _inputActions.UI.Click.Disable();
                _inputActions.UI.Click.performed -= OnClickPerformed;
            }
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            _lastMovementInput = context.ReadValue<Vector2>();
            if (_lockedControls) return;
            OnMovement?.Invoke(_lastMovementInput);
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext context) {
            if (_lockedControls) return;
            OnInteract?.Invoke();
        }
        
        private void OnInteractCancelled(InputAction.CallbackContext context) {
            if (_lockedControls) return;
            OnInteractCancel?.Invoke();
        }

        private void OnCancelPerformed(InputAction.CallbackContext context) {
            OnCancel?.Invoke();
        }

        private void OnClickPerformed(InputAction.CallbackContext context) {
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            Vector2 clickPosition = Mouse.current.position.ReadValue();
            OnClick?.Invoke(clickPosition);
            OnClickWorld?.Invoke(Camera.main.ScreenToWorldPoint(clickPosition));
        }
        
        private void OnPointPerformed(InputAction.CallbackContext context) {
            OnPoint?.Invoke();
        }
        
        private void OnLevelStart() {
            _lockedControls = false;
            OnMovement?.Invoke(_lastMovementInput);
        }
        private void OnLevelOver() {
            _lockedControls = true;
            OnMovement?.Invoke(Vector2.zero);
        }
    }
}