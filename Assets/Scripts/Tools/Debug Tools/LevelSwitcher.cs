using System;

using AppCore;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Tools.Debug_Tools {
    public class LevelSwitcher : MonoBehaviour {
        [SerializeField] private bool enabled = true;
        
        // Unity functions
        private void OnEnable() {
            App.InputManager.OnLevelSelect += OnLevelSelect;
        }

        private void OnDisable() {
            App.InputManager.OnLevelSelect -= OnLevelSelect;
        }
        
        // Private functions
        private void OnLevelSelect(int level) {
            if (!enabled) return;
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (lm != null) {
                lm.LoadLevel(level);
            }
        }
    }
}