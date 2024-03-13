using System;

using Game.GameManagement;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerAnimator : MonoBehaviour {
        private Animator _animator;
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }

        private void Awake() {
            _animator = GetComponent<Animator>();
        }
        
        // Private functions
        private void OnLevelStart() {
            _animator.SetTrigger(AnimationConstants.Player.Idle);
        }
        
        private void OnLevelOver() {
            _animator.SetTrigger(AnimationConstants.Player.Die);
        }
        
        // Public functions
        public void SetDirection(Vector2 direction) {
            _animator.ResetTrigger(AnimationConstants.Player.FaceRight);
            _animator.ResetTrigger(AnimationConstants.Player.FaceLeft);
            _animator.ResetTrigger(AnimationConstants.Player.FaceUp);
            _animator.ResetTrigger(AnimationConstants.Player.FaceDown);
            _animator.ResetTrigger(AnimationConstants.Player.Idle);
            
            if (direction.x > 0) {
                _animator.SetTrigger(AnimationConstants.Player.FaceRight);
            } else if (direction.x < 0) {
                _animator.SetTrigger(AnimationConstants.Player.FaceLeft);
            } else if (direction.y > 0) {
                _animator.SetTrigger(AnimationConstants.Player.FaceUp);
            } else if (direction.y < 0) {
                _animator.SetTrigger(AnimationConstants.Player.FaceDown);
            } else {
                _animator.SetTrigger(AnimationConstants.Player.Idle);
            }
        }
    }
}