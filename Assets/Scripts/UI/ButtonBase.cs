using System;

using AppCore;
using AppCore.AudioManagement;

using UnityEngine;

namespace UI {
    public class ButtonBase : MonoBehaviour { // Base class for all buttons in the game, mostly manages audio
        [SerializeField] private SoundPackage hoverSound;
        [SerializeField] private SoundPackage clickSound;
        
        private bool _isSelected = false;
        
        private static Action<ButtonBase> s_onNewSelection;
        
        // Unity functions
        private void OnEnable() {
            s_onNewSelection += OnNewSelection;
        }
        
        private void OnDisable() {
            s_onNewSelection -= OnNewSelection;
        }
        
        // Private functions
        private void OnNewSelection(ButtonBase button) {
            if (button != this) {
                OnDeselect();
            }
        }

        // Event trigger functions
        public void OnSubmit() {
            App.AudioManager.PlaySFX(clickSound);
        }

        public void OnPointerClick() {
            OnSubmit();
        }

        public void OnPointerEnter() {
            OnSelect();
        }
        
        public void OnPointerExit() {
            _isSelected = false;
        }
        
        public void OnSelect() {
            if (_isSelected) return;
            App.AudioManager.PlaySFX(hoverSound);
            _isSelected = true;
            s_onNewSelection?.Invoke(this);
        }

        public void OnDeselect() {
            _isSelected = false;
        }
    }
}