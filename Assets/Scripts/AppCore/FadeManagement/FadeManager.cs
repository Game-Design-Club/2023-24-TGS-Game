using System;

using UnityEngine;

namespace AppCore.FadeManagement {
    public class FadeManager : MonoBehaviour {
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