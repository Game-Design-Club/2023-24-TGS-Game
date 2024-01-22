using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : PlayerMovement {
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private bool _isTouchingBox;
        private bool _isGrabbingBox;
        
        private Vector2 _currentMovementDirection;
        private Vector2 _currentMovementInput;
        
        // Unity functions
        private void OnTriggerEnter2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                _isTouchingBox = true;
            }
        } 
        
        private void OnTriggerExit2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                _isTouchingBox = false;
            }
        }
        
        // Public functions
        public void MovementChanged(Vector2 movementInput) {
            if (!_isGrabbingBox) {
                // base.MovementChanged(movementInput);
                return;
            }
        }
    }
}