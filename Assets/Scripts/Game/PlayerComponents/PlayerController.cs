using AppCore;

using Game.GameManagement;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerController : MonoBehaviour {
        private PlayerMovement _playerMovement;
        private bool _lockedControls;
        
        // Unity functions
        private void Awake() {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        
        private void OnEnable() {
            App.Instance.inputManager.OnMovementInput += MovementInputChange;
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            App.Instance.inputManager.OnMovementInput -= MovementInputChange;
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            GameManager.Instance.OnLevelOver -= OnLevelOver;
        }
        
        // Private functions
        private void MovementInputChange(Vector2 movementInput) {
            if (_lockedControls) return;
            _playerMovement.MovementChanged(movementInput);
        }
        
        private void OnLevelStart() {
            _lockedControls = false;
        }
        
        private void OnLevelOver() {
            _lockedControls = true;
            _playerMovement.MovementChanged(Vector2.zero);
        }
    }
}