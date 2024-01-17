using System;

using Game.GameManagement;

using Unity.VisualScripting;
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
        
        // Player
        public event Action<Vector2> OnMovement;
        public event Action OnInteract;
        
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
            void EnableMovement() {
                _inputActions.Player.Move.Enable();
                _inputActions.Player.Move.performed += OnMovementPerformed;
                _inputActions.Player.Move.canceled += OnMovementPerformed;
            }
            void EnableInteract() {
                _inputActions.Player.Interact.Enable();
                _inputActions.Player.Interact.performed += OnInteractPerformed;
            }
            void EnableCancel() {
                _inputActions.UI.Cancel.Enable();
                _inputActions.UI.Cancel.performed += OnCancelPerformed;
            }
            void EnableClicking() {
                _inputActions.UI.Click.Enable();
                _inputActions.UI.Click.performed += OnClickPerformed;
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

        private void OnCancelPerformed(InputAction.CallbackContext context) {
            OnCancel?.Invoke();
        }

        private void OnClickPerformed(InputAction.CallbackContext context) {
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            Vector2 clickPosition = Mouse.current.position.ReadValue();
            OnClick?.Invoke(clickPosition);
            OnClickWorld?.Invoke(Camera.main.ScreenToWorldPoint(clickPosition));
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