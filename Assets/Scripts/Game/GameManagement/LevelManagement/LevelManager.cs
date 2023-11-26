using System;
using System.Collections;

using AppCore;

using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private Level firstLevel;
        [SerializeField] private Level customFirstLevel;

        private Level _currentLevel;
        private GameObject _levelGameObject;

        private bool _currentlySwitching;
        
        public event Action OnLevelLoaded;
        public static LevelManager Instance { get; private set; }
        
        // Unity functions
        private void Awake() {
            if (Instance is null) {
                Instance = this;
            } else {
                Debug.LogWarning("Duplicate LevelManager found and deleted.");
                Destroy(gameObject);
            }
            
            if (customFirstLevel != null) {
                firstLevel = customFirstLevel;
            }
        }

        // Public functions
        public void LoadFirstLevel() {
            StartCoroutine(LoadLevel(firstLevel, false));
        }
        
        public void LoadNextLevel() {
            if (_currentLevel.nextLevel is null) {
                Debug.LogWarning($"Next level for '{_currentLevel}' is not assigned");
                return;
            }
            StartCoroutine(LoadLevel(_currentLevel.nextLevel));
        }
        
        public void RestartLevel() {
            StartCoroutine(LoadLevel(_currentLevel));
        }
        
        public IEnumerator LoadLevel(Level level, bool fade = true) {
            if (_currentlySwitching) {
                Debug.LogWarning("Tried to load level in the middle of loading another level");
                yield break;
            }

            if (level == null) {
                Debug.LogWarning("Tried to load a null level");
                yield break;
            }
            if (fade) {
                App.Instance.fadeManager.FadeIn();
                _currentlySwitching = true;
                yield return new WaitForSecondsRealtime(App.Instance.fadeManager.transitionPeriod);
                _currentlySwitching = false;
                App.Instance.fadeManager.FadeOut();
            }
            ChangeCurrentLevel(level);
        }

        private void ChangeCurrentLevel(Level level) {
            if (_currentLevel != null) {
                Destroy(_levelGameObject);
            }
            if (level.gameObject == null) {
                Debug.LogWarning($"Level '{level}' has no game object assigned");
            } else {
                _levelGameObject = Instantiate(level.gameObject);
            }
            _currentLevel = level;
            OnLevelLoaded?.Invoke();
        }
    }
}