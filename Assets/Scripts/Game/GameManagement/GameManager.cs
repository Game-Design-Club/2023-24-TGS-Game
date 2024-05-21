using AppCore;
using AppCore.AudioManagement;

using Game.GameManagement.LevelManagement;

using Tools.Constants;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour { // Manages the essential game functions, proxies to other managers
        private LevelManager _levelManager;

        private static GameManager s_instance;
        
        private bool _freeze = false;
        
        // Unity functions
        private void Awake() {
            if (s_instance is null) {
                s_instance = this;
            } else {
                Debug.LogWarning("Duplicate GameManager found and deleted.");
                Destroy(gameObject);
            }
            
            _levelManager = GetComponentInChildren<LevelManager>();
            
            if (_levelManager is null) {
                Debug.LogError("LevelManager not found.");
            }
        }

        private void Start() {
            _levelManager.LoadSavedLevel();
            App.TimerManager.StartTimer();
        }

        private void OnDestroy() {
            if (s_instance == this) {
                s_instance = null;
            }
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        // Private functions
        private void InternalPlayerDied() {
            RestartLevel();
        }
        
        private void InternalLevelCompleted() {
            _levelManager.LoadNextLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        // Public functions
        
        public void RestartLevel() {
            if (_freeze) return;
            _freeze = true;
            
            _levelManager.RestartLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        public void QuitToMainMenu() {
            if (_freeze) return;
            _freeze = true;
            
            App.SceneManager.LoadScene(SceneConstants.MainMenu);
        }
        
        private void OnLevelStart() {
            _freeze = false;
        }
        
        // Static functions
        public static void PlayerDied() {
            s_instance.InternalPlayerDied();
        }
        
        public static void LevelCompleted() {
            s_instance.InternalLevelCompleted();
        }
    }
}