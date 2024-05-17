using System;

using AppCore;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerAnimator : MonoBehaviour { // Player animator
        [SerializeField] private Transform playerSprite;
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private PlayerBoxMover _playerBoxMover;
        
        // Unity functions
        private void Awake() {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerBoxMover = GetComponent<PlayerBoxMover>();
        }

        private void OnEnable() {
            App.InputManager.OnMovement += SetDirection;
            _playerBoxMover.onBoxGrabbed += BoxGrabbed;
        }
        
        private void OnDisable() {
            App.InputManager.OnMovement -= SetDirection;
            _playerBoxMover.onBoxGrabbed -= BoxGrabbed;
        }

        // Public functions
        public void PlayDeathAnimation() {
            _animator.SetTrigger(AnimationConstants.Player.Die);
        }
        
        // Private functions
        private void SetDirection(Vector2 movement) {
            if (movement == Vector2.zero) return;
            float angle;
            angle = GetAngleFromDirection(_playerBoxMover.IsGrabbingBox ? -_playerBoxMover.AttachDirection : movement);
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void BoxGrabbed(bool isGrabbing) {
            SetDirection(_playerMovement.CurrentMovementInput);
        }
        
        private float GetAngleFromDirection(Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) {
                angle += 360;
            }
            return angle;
        }
    }
}