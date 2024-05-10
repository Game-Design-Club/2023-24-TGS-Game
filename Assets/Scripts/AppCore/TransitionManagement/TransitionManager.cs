using Tools.Constants;

using UnityEngine;

namespace AppCore.TransitionManagement {
    public class TransitionManager : MonoBehaviour { // Manages transitions between scenes with fun animations
        [SerializeField] public float fadeTime = 2f;
        [SerializeField] public float wipeTime = 1.5f;
        
        private Animator _animator;
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void FadeIn(TransitionType transitionType = TransitionType.Fade) {
            ResetTriggers();
            switch (transitionType) {
                case TransitionType.Fade:
                    _animator.SetTrigger(AnimationConstants.FadeInOut.FadeToBlack);
                    break;
                case TransitionType.Wipe:
                    _animator.SetTrigger(AnimationConstants.WipeInOut.WipeToBlack);
                    break;
            }
        }
        
        public void FadeOut(TransitionType transitionType = TransitionType.Fade) {
            ResetTriggers();
            switch (transitionType) {
                case TransitionType.Fade:
                    _animator.SetTrigger(AnimationConstants.FadeInOut.FadeFromBlack);
                    break;
                case TransitionType.Wipe:
                    _animator.SetTrigger(AnimationConstants.WipeInOut.WipeFromBlack);
                    break;
            }
        }
        
        // Private functions
        private void ResetTriggers() {
            _animator.ResetTrigger(AnimationConstants.FadeInOut.FadeToBlack);
            _animator.ResetTrigger(AnimationConstants.FadeInOut.FadeFromBlack);
            _animator.ResetTrigger(AnimationConstants.WipeInOut.WipeToBlack);
            _animator.ResetTrigger(AnimationConstants.WipeInOut.WipeFromBlack);
        }
    }
}