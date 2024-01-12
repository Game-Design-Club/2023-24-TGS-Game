using System;

using AppCore;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour{
        public static GameManager Instance { get; private set; }
        
        public event Action OnLevelStart; // LevelStart is called when the level is loaded
        public event Action OnLevelOver; // LevelOver is called when the player dies or finishes the level
        
        private LevelManager _levelManager;
        
        // Unity functions
        private void Awake() {
            if (Instance is null) {
                Instance = this;
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
            RestartLevel();
        }
        
        public void RestartLevel() {
            _levelManager.RestartLevel();
            OnLevelOver?.Invoke();
        }
        
        public void LevelCompleted() {
            _levelManager.LoadNextLevel();
            OnLevelOver?.Invoke();
        }
        
        public void LoadMainMenu() {
            App.Instance.sceneManager.LoadScene(Constants.SceneConstants.MainMenu);
        }
    }
}