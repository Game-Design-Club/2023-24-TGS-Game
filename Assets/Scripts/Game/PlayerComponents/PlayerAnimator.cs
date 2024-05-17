using System;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerAnimator : MonoBehaviour { // Player animator
        [SerializeField] private Transform playerSprite;
        private Animator _animator;
        private PlayerMovement _playerMovement;
        
        // Unity functions
        
        private void Awake() {
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable() {
            _playerMovement.OnPlayerMoved += SetDirection;
        }
        
        private void OnDisable() {
            _playerMovement.OnPlayerMoved -= SetDirection;
        }

        // Public functions
        public void PlayDeathAnimation() {
            _animator.SetTrigger(AnimationConstants.Player.Die);
        }

        public void SetDirection(Vector2 currentMovement, bool extra) {
            if (extra) return;
            if (currentMovement == Vector2.zero) return;
            float angle = GetAngleFromDirection(currentMovement);
            Debug.Log(angle);
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        // Private functions
        private float GetAngleFromDirection(Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) {
                angle += 360;
            }
            return angle;
        }
    }
}