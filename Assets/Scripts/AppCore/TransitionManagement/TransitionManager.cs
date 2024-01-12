using UnityEngine;

namespace AppCore.TransitionManagement {
    public class TransitionManager : MonoBehaviour {
        [SerializeField] public float fadeTime = 2f;
        [SerializeField] public float wipeTime = 1.5f;
        
        private Animator _animator;
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void FadeIn(TransitionType transitionType = TransitionType.Fade) {
            switch (transitionType) {
                case TransitionType.Fade:
                    _animator.SetTrigger(Constants.AnimationConstants.FadeInOut.FadeToBlack);
                    break;
                case TransitionType.Wipe:
                    _animator.SetTrigger(Constants.AnimationConstants.WipeInOut.WipeToBlack);
                    break;
            }
        }
        
        public void FadeOut(TransitionType transitionType = TransitionType.Fade) {
            switch (transitionType) {
                case TransitionType.Fade:
                    _animator.SetTrigger(Constants.AnimationConstants.FadeInOut.FadeFromBlack);
                    break;
                case TransitionType.Wipe:
                    _animator.SetTrigger(Constants.AnimationConstants.WipeInOut.WipeFromBlack);
                    break;
            }
        }
    }
}