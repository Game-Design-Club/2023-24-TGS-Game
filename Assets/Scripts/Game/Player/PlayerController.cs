using AppCore;

using UnityEngine;

namespace Game.Player {
    public class PlayerController : MonoBehaviour{
        private PlayerMovement _playerMovement;
        
        private void Awake() {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        
        private void OnEnable() {
            App.Instance.inputManager.OnMovementInput += OnMovementInput;
        }
        
        private void OnDisable() {
            App.Instance.inputManager.OnMovementInput -= OnMovementInput;
        }
        
        private void OnMovementInput(Vector2 movementInput) {
            _playerMovement.MovementChanged(movementInput);
        }
    }
}