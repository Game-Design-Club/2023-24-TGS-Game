using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerAnimator : MonoBehaviour{
        private Animator _animator;
        
        // Unity functions
        
        private void Awake() {
            _animator = GetComponent<Animator>();
        }
        
        // Public functions
        public void PlayDeathAnimation() {
            _animator.SetTrigger(AnimationConstants.Player.Die);
        }
    }
}