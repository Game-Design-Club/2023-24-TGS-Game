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
        
        public bool LockedUI {
            get {
                return LockedUIList.Count > 0;
            }
        }

        private readonly List<object> LockedControlsList = new List<object>();
        private readonly List<object> LockedUIList = new List<object>();
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
        
        // Special
        public event Action OnDialogueContinue;
        public event Action OnPlayerStartMovement;
        
        // Levels
        public event Action<int> OnLevelSelect;
        
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
            EnableLevelSelect();
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
            void EnableLevelSelect() {
                _inputActions.Debug.Level1.Enable();
                _inputActions.Debug.Level1.performed += LevelSelect(0);
                _inputActions.Debug.Level2.Enable();
                _inputActions.Debug.Level2.performed += LevelSelect(1);
                _inputActions.Debug.Level3.Enable();
                _inputActions.Debug.Level3.performed += LevelSelect(2);
                _inputActions.Debug.Level4.Enable();
                _inputActions.Debug.Level4.performed += LevelSelect(3);
                _inputActions.Debug.Level5.Enable();
                _inputActions.Debug.Level5.performed += LevelSelect(4);
                _inputActions.Debug.Level52.Enable();
                _inputActions.Debug.Level52.performed += LevelSelect(5);
                _inputActions.Debug.Level6.Enable();
                _inputActions.Debug.Level6.performed += LevelSelect(6);
            }
        }
        private void DisableAll() {
            DisableMovement();
            DisableInteract();
            DisableCancel();
            DisableClicking();
            DisableLevelSelect();
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

            void DisableLevelSelect() {
                _inputActions.Debug.Level1.Disable();
                _inputActions.Debug.Level1.performed -= LevelSelect(0);
                _inputActions.Debug.Level2.Disable();
                _inputActions.Debug.Level2.performed -= LevelSelect(1);
                _inputActions.Debug.Level3.Disable();
                _inputActions.Debug.Level3.performed -= LevelSelect(2);
                _inputActions.Debug.Level4.Disable();
                _inputActions.Debug.Level4.performed -= LevelSelect(3);
                _inputActions.Debug.Level5.Disable();
                _inputActions.Debug.Level5.performed -= LevelSelect(4);
                _inputActions.Debug.Level52.Disable();
                _inputActions.Debug.Level52.performed -= LevelSelect(5);
                _inputActions.Debug.Level6.Disable();
                _inputActions.Debug.Level6.performed -= LevelSelect(6);
            }
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            _lastMovementInput = context.ReadValue<Vector2>();
            if (LockedControls) return;
            OnMovement?.Invoke(_lastMovementInput);
        }
        
        private void OnInteractPerformed(InputAction.CallbackContext context) {
            OnDialogueContinue?.Invoke();
            if (LockedControls) return;
            OnInteract?.Invoke();
        }
        
        private void OnInteractCancelled(InputAction.CallbackContext context) {
            if (LockedControls) return;
            OnInteractCancel?.Invoke();
        }

        private void OnCancelPerformed(InputAction.CallbackContext context) {
            if (LockedUI) return;
            OnCancel?.Invoke();
        }

        private void OnClickPerformed(InputAction.CallbackContext context) {
            if (!Mouse.current.leftButton.wasPressedThisFrame) return;
            Camera cam = Camera.main;
            Vector2 clickPosition = Mouse.current.position.ReadValue();
            if (cam.pixelRect.Contains(clickPosition)) {
                OnDialogueContinue?.Invoke();
                if (LockedUI) return;
                OnClick?.Invoke(clickPosition);
                OnClickWorld?.Invoke(cam.ScreenToWorldPoint(clickPosition));
            } else {
                if (LockedUI) return;
                OnClick?.Invoke(clickPosition);
            }
        }
        
        private Action<InputAction.CallbackContext> LevelSelect(int level) {
            return (context) => {
                if (LockedUI) return;
                OnLevelSelect?.Invoke(level);
            };
        }
        
        private void OnPointPerformed(InputAction.CallbackContext context) {
            if (LockedUI) return;
            OnPoint?.Invoke();
        }
        
        private void OnSubmitPerformed(InputAction.CallbackContext context) {
            OnDialogueContinue?.Invoke();
            if (LockedUI) return;
            OnSubmit?.Invoke();
        }
        
        private void OnLevelStart() {
            StartCoroutine(UnlockControlsAfterSeconds(App.TransitionManager.wipeTime));
        }
        private IEnumerator UnlockControlsAfterSeconds(float seconds) {
            yield return new WaitForSecondsRealtime(seconds);
            LockedControlsList.Remove(this);
            OnPlayerStartMovement?.Invoke();
            yield return new WaitUntil(() => !LockedControls);
            OnMovement?.Invoke(_lastMovementInput);
        }
        private void OnLevelOver() {
            LockedControlsList.Add(this);
            OnMovement?.Invoke(Vector2.zero);
        }
        
        // Public functions
        public void LockPlayerControls(System.Object caller) {
            LockedControlsList.Add(caller);
            OnMovement?.Invoke(Vector2.zero);
        }
        
        public void UnlockPlayerControls(System.Object caller) {
            LockedControlsList.Remove(caller);
            if (!LockedControls) {
                OnMovement?.Invoke(_lastMovementInput);
            }
        }
        
        public void LockUI(System.Object caller) {
            LockedUIList.Add(caller);
        }
        
        public void UnlockUI(System.Object caller) {
            LockedUIList.Remove(caller);
        }
    }
}