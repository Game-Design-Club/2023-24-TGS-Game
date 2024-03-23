using System;
using System.Collections;

using AppCore;
using AppCore.TransitionManagement;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameManagement.LevelManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private LevelsData levelsData;
        [SerializeField] private Level customFirstLevel;

        private Level _currentLevel;
        private int _currentLevelIndex;
        private GameObject _levelGameObject;

        public static bool IsCurrentlySwitching;

        // Public functions
        public void LoadFirstLevel() {
            StartCoroutine(LoadLevel(levelsData[0], false));
        }
        
        public void LoadSavedLevel() {
            if (levelsData == null) {
                Debug.LogError("No levels data assigned to the level manager");
                return;
            }
            StartCoroutine(LoadLevel(levelsData[App.Instance.playerDataManager.LastCompletedLevelIndex], false));
        }
        
        public void LoadNextLevel() {
            _currentLevelIndex++;
            if (levelsData[_currentLevelIndex] == null) {
                // If there are no more levels, go to the credits
                _currentLevelIndex = 0;
                App.Instance.playerDataManager.LastLevelCompleted(_currentLevelIndex);
                App.Instance.sceneManager.LoadScene(SceneConstants.Credits);
                return;
            }
            StartCoroutine(LoadLevel(levelsData[_currentLevelIndex]));
            App.Instance.playerDataManager.LastLevelCompleted(_currentLevelIndex);
        }
        
        public void RestartLevel() {
            StartCoroutine(LoadLevel(_currentLevel));
        }
        
        public IEnumerator LoadLevel(Level level, bool fade = true) {
            if (IsCurrentlySwitching) {
                Debug.LogWarning("Tried to load level in the middle of loading another level");
                yield break;
            }

            if (level == null) {
                Debug.LogWarning("Tried to load a null level");
                yield break;
            }
            if (fade) {
                App.Instance.transitionManager.FadeIn(TransitionType.Wipe);
                IsCurrentlySwitching = true;
                yield return new WaitForSecondsRealtime(App.Instance.transitionManager.wipeTime);
                IsCurrentlySwitching = false;
                App.Instance.transitionManager.FadeOut(TransitionType.Wipe);
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
            GameManagerEvents.InvokeLevelStart();
        }
    }
}