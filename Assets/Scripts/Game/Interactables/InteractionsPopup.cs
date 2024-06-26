using System;

using TMPro;

using Tools.Constants;

using UnityEngine;

namespace Game.Interactables {
    public class InteractionsPopup : MonoBehaviour { // Manages the interactions popup
        private static InteractionsPopup s_instance;

        private Animator _animator;
        private TextMeshProUGUI _text;
        
        private bool _showing = false;
        
        // Unity functions
        private void Awake() {
            if (s_instance == null) {
                s_instance = this;
            } else {
                Debug.Log("InteractionsPopup already exists. Deleting this one.");
                Destroy(gameObject);
            }

            _animator = GetComponent<Animator>();
            if (_animator == null) {
                Debug.LogWarning("InteractionsPopup has no Animator component.");
            }
            
            _text = GetComponentInChildren<TextMeshProUGUI>();
            if (_text == null) {
                Debug.LogWarning("InteractionsPopup has no TextMeshProUGUI component.");
            }
        }

        private void OnDestroy() {
            if (s_instance == this) {
                s_instance = null;
            }
        }
        
        private void InternalShow(String message) {
            if (_showing) return;
            _showing = true;
            _text.text = message;
            _animator.SetTrigger(AnimationConstants.InteractionsPopup.Show);
        }
        
        private void InternalHide() {
            if (!_showing) return;
            _showing = false;
            _animator.SetTrigger(AnimationConstants.InteractionsPopup.Hide);
        }
        
        // Public functions
        public static void Show(String message = "Press Space to Interact") {
            if (!s_instance) return;
            s_instance.InternalShow(message);
        }
        
        public static void Hide() {
            if (!s_instance) return;
            s_instance.InternalHide();
        }
    }
}