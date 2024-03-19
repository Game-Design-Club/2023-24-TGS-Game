using System;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Game.GameManagement.PauseManagement {
    public class PauseManager : MonoBehaviour{
        public static bool IsPaused { get; private set; }
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnApplicationFocus(bool hasFocus) {
            if (!hasFocus && !IsPaused) {
                PauseGame();
            }
        }

        // Public functions
        public void PauseGame() {
            if (IsPaused) {
                Debug.LogWarning("Tried to pause game while already paused.");
                return;
            }
            PauseManagerEvents.InvokeGamePause();
            IsPaused = true;
            Time.timeScale = 0;
        }
        
        public void ResumeGame() {
            if (!IsPaused) {
                Debug.LogWarning("Tried to resume game while not paused.");
                return;
            }
            if (LevelManager.IsCurrentlySwitching) return;
            PauseManagerEvents.InvokeGameResume();
            IsPaused = false;
            Time.timeScale = 1;
        }
        
        public void OnLevelStart() {
            if (IsPaused) {
                ResumeGame();
            } else {
                Time.timeScale = 1;
            }
        }
        
        public void TogglePaused() {
            if (IsPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }
}