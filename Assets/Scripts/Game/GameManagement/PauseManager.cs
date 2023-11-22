using System;

using UnityEngine;

namespace Game.GameManagement {
    public class PauseManager : MonoBehaviour{
        private bool IsPaused { get; set; }
        
        public event Action OnGamePause;
        public event Action OnGameResume;
        
        public static PauseManager Instance { get; private set; }
        
        // Unity functions
        private void Awake() {
            if (Instance is null) {
                Instance = this;
            } else {
                Debug.LogWarning("Duplicate PauseManager found and deleted.");
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            GameManager.Instance.OnLevelStart += TryResumeGame;
        }
        
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= TryResumeGame;
        }
        
        // Public functions
        public void PauseGame() {
            if (IsPaused) {
                Debug.LogWarning("Tried to pause game while already paused.");
                return;
            }
            OnGamePause?.Invoke();
            IsPaused = true;
            Time.timeScale = 0;
        }
        
        public void ResumeGame() {
            if (!IsPaused) {
                Debug.LogWarning("Tried to resume game while not paused.");
                return;
            }
            OnGameResume?.Invoke();
            IsPaused = false;
            Time.timeScale = 1;
        }
        
        public void TryResumeGame() {
            if (IsPaused) {
                ResumeGame();
            }
        }
    }
}