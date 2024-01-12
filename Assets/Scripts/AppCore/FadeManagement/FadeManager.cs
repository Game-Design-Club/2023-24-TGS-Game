using UnityEditor.Animations;

using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.FadeManagement {
    public class FadeManager : MonoBehaviour {
        [SerializeField] public float transitionPeriod = 1.5f;
        [SerializeField] private AnimatorController wipeAnimator;
        
        private AnimatorController _currentAnimator;
        private Animator _animator;
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void FadeIn() {
            _animator.SetTrigger(Constants.AnimationConstants.FadeInOut.FadeIn);
        }
        
        public void FadeOut() {
            _animator.SetTrigger(Constants.AnimationConstants.FadeInOut.FadeOut);
        }
    }
}