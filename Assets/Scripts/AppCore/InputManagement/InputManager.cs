using System;
using System.Collections;
using System.Collections.Generic;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore.InputManagement { // This class is used to manage all player input in the game
    public class InputManager : MonoBehaviour {
        private InputActions _inputActions;
        private Vector2 _lastMovementInput;

        public bool LockedControls {
            get {
                return LockedControlsList.Count > 0;
            }
        }
        
        public readonly List<object> LockedControlsList = new List<object>();
        // Scripts need to log themselves to lock controls or ui, then remove themselves when they're done
        // This way multiple scripts can lock controls at the same time, and one removing it won't remove the other
        
        // UI
        public event Action OnCancel;
        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnClickWorld;
        public event Action OnPoint;
        public event Action OnSubmit;
        
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
            EnableUIInteract();

            return;
            
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
            void EnableUIInteract() {
                _inputActions.UI.Submit.Enable();
                _inputActions.UI.Submit.performed += OnSubmitPerformed;
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
            if (LockedControls) return;
            OnMovement?.Invoke(_lastMovementInput);
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext context) {
            if (LockedControls) return;
            OnInteract?.Invoke();
        }
        
        private void OnInteractCancelled(InputAction.CallbackContext context) {
            if (LockedControls) return;
            OnInteractCancel?.Invoke();
        }

        private void OnCancelPerformed(InputAction.CallbackContext context) {
            OnCancel?.Invoke();
        }

        private void OnClickPerformed(InputAction.CallbackContext context) {
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            Vector2 clickPosition = Mouse.current.position.ReadValue();
            OnClick?.Invoke(clickPosition);
            Camera cam = Camera.main;
            if (cam.pixelRect.Contains(clickPosition)) {
                OnClickWorld?.Invoke(cam.ScreenToWorldPoint(clickPosition));
            }
        }
        
        private void OnPointPerformed(InputAction.CallbackContext context) {
            OnPoint?.Invoke();
        }
        
        private void OnSubmitPerformed(InputAction.CallbackContext context) {
            OnSubmit?.Invoke();
        }
        
        private void OnLevelStart() {
            StartCoroutine(UnlockControlsAfterSeconds(App.TransitionManager.wipeTime));
        }
        private IEnumerator UnlockControlsAfterSeconds(float seconds) {
            yield return new WaitForSecondsRealtime(seconds);
            LockedControlsList.Remove(this);
            yield return new WaitUntil(() => !LockedControls);
            OnMovement?.Invoke(_lastMovementInput);
        }
        private void OnLevelOver() {
            LockedControlsList.Add(this);
            OnMovement?.Invoke(Vector2.zero);
        }
    }
}