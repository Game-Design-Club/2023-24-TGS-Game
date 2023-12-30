using System;

using Constants;

using UnityEngine;

namespace Game.Interactables {
    public class InteractionsPopup : MonoBehaviour{
        public static InteractionsPopup Instance { get; private set; }

        private Animator _animator;
        
        // Unity functions
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.Log("InteractionsPopup already exists. Deleting this one.");
                Destroy(gameObject);
            }

            _animator = GetComponent<Animator>();
            if (_animator == null) {
                Debug.LogWarning("InteractionsPopup has no Animator component.");
            }
        }
        
        public void Show() {
            _animator.SetTrigger(AnimationConstants.InteractionsPopup.Show);
        }
        
        public void Hide() {
            _animator.SetTrigger(AnimationConstants.InteractionsPopup.Hide);
        }
    }
}