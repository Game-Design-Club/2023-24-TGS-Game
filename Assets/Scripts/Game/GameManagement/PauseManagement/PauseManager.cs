using UnityEngine;

namespace Game.GameManagement.PauseManagement {
    public class PauseManager : MonoBehaviour{
        private bool IsPaused { get; set; }
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
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
            PauseManagerEvents.InvokeGameResume();
            IsPaused = false;
            Time.timeScale = 1;
        }
        
        public void OnLevelStart() {
            if (IsPaused) {
                ResumeGame();
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