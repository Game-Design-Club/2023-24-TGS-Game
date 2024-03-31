using AppCore;
using AppCore.AudioManagement;

using Game.GameManagement.LevelManagement;

using Tools.Constants;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour { // Manages the essential game functions, proxies to other managers
        [SerializeField] private SoundPackage[] ambienceSounds;
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
            App.Instance.audioManager.musicPlayer.PlayGameMusic();
            foreach (SoundPackage soundPackage in ambienceSounds) {
                App.Instance.audioManager.PlaySFX(soundPackage, parent: transform);
            }
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

        // Public functions
        public void PlayerDied() {
            RestartLevel();
        }
        
        public void RestartLevel() {
            if (_freeze) return;
            _freeze = true;
            
            _levelManager.RestartLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        public void LevelCompleted() {
            _levelManager.LoadNextLevel();
            GameManagerEvents.InvokeLevelOver();
        }
        
        public void QuitToMainMenu() {
            if (_freeze) return;
            _freeze = true;
            
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu);
        }
        
        private void OnLevelStart() {
            _freeze = false;
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