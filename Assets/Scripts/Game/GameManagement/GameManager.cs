using System;

using AppCore;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour{
        public static GameManager Instance { get; private set; }
        
        public event Action OnLevelStart;
        public event Action OnLevelOver;
        public event Action OnGamePause;
        public event Action OnGameResume;

        private LevelManager _levelManager;
        
        public bool IsPaused { get; private set; } // isPaused should only be true if isPlaying is true
        
        // Unity functions
        private void Awake() {
            if (Instance is null) {
                Instance = this;
                Debug.Log("GameManager initialized.");
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

        // Public functions
        public void GameStart() {
            OnLevelStart?.Invoke();
        }
        
        public void PlayerDied() {
            _levelManager.RestartLevel();
            OnLevelOver?.Invoke();
        }
        
        public void LevelFinished() {
            _levelManager.LoadNextLevel();
            OnLevelOver?.Invoke();
        }
        
        public void GamePause() {
            if (IsPaused) {
                Debug.LogWarning("Tried to pause game while already paused.");
                return;
            }
            OnGamePause?.Invoke();
            IsPaused = true;
            Time.timeScale = 0;
        }
        
        public void GameResume() {
            if (!IsPaused) {
                Debug.LogWarning("Tried to resume game while not paused.");
                return;
            }
            OnGameResume?.Invoke();
            IsPaused = false;
            Time.timeScale = 1;
        }
        
        public void LoadMainMenu() {
            Debug.LogError("Not implemented yet");
            // App.Instance.sceneManager.LoadScene(Constants.SceneConstants.MainMenu);
        }
    }
}