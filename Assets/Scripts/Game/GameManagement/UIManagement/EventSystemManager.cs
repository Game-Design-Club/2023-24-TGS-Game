using AppCore;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameManagement.UIManagement
{
    public class EventSystemManager : MonoBehaviour {
        private EventSystem _eventSystem;
        private enum InputType { Mouse, Keyboard }
        private InputType _inputType = InputType.Mouse;
        
        private void OnEnable() {
            App.Instance.inputManager.OnPoint += HandleMouseMovement;
            App.Instance.inputManager.OnMovement += HandleKeyboardInput;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnPoint -= HandleMouseMovement;
            App.Instance.inputManager.OnMovement -= HandleKeyboardInput;
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
    }
}