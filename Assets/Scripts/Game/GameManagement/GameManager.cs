using System;

using AppCore;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour{
        
        private LevelManager _levelManager;

        private static GameManager s_instance;
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

        private void OnEnable() {
            _levelManager.OnLevelLoaded += GameStart;
        }
        
        private void OnDisable() {
            _levelManager.OnLevelLoaded -= GameStart;
        }

        private void Start() {
            _levelManager.LoadFirstLevel();
        }

        private void OnDestroy() {
            if (s_instance == this) {
                s_instance = null;
            }
        }

        // Public functions
        public void GameStart() {
            GameManagerEvents.InvokeLevelStart();
        }
        
        public void PlayerDied() {
            RestartLevel();
        }
        
        public void RestartLevel() {
            _levelManager.RestartLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        public void LevelCompleted() {
            _levelManager.LoadNextLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        public void QuitToMainMenu() {
            App.Instance.sceneManager.LoadScene(Constants.SceneConstants.MainMenu);
        }
        
        // Static functions
        public static void PlayerDiedStatic() {
            s_instance.PlayerDied();
        }
        
        public static void LevelCompletedStatic() {
            s_instance.LevelCompleted();
        }
    }
}