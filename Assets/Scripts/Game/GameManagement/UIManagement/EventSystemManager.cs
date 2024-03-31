using AppCore;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameManagement.UIManagement
{
    public class EventSystemManager : MonoBehaviour { // Manages the event system, selecting objects and switching between input methods
        private EventSystem _eventSystem;
        private enum InputType { Mouse, Keyboard }
        private InputType _inputType = InputType.Mouse;
        
        private void OnEnable() {
            App.InputManager.OnPoint += HandleMouseMovement;
            App.InputManager.OnMovement += HandleKeyboardInput;
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }

        private void OnDisable() {
            App.InputManager.OnPoint -= HandleMouseMovement;
            App.InputManager.OnMovement -= HandleKeyboardInput;
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void Awake() {
            _eventSystem = GetComponent<EventSystem>();
        }

        private void HandleMouseMovement() {
            _eventSystem.SetSelectedGameObject(null);
            Cursor.visible = true;
            _inputType = InputType.Mouse;
        }

        private void HandleKeyboardInput(Vector2 movementInput) {
            if (_inputType == InputType.Keyboard) return;
            
            Cursor.visible = false;
            _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
            _inputType = InputType.Keyboard;
        }
        
        private void OnLevelStart() {
            // Only in game scene
            _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
        }
        
        // Public functions
        public void SetSelectedGameObject(GameObject selectedGameObject, bool alsoSetDefault = true) {
            if (selectedGameObject == null) {
                _eventSystem.SetSelectedGameObject(null);
                Debug.LogWarning("SelectedGameObject is null");
                return;
            }
            _eventSystem.SetSelectedGameObject(selectedGameObject);
            if (alsoSetDefault) {
                _eventSystem.firstSelectedGameObject = selectedGameObject;
            }
        }
    }
}