using System;
using System.Collections;

using AppCore;
using AppCore.TransitionManagement;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameManagement.LevelManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private Level firstLevel;
        [SerializeField] private Level customFirstLevel;

        private Level _currentLevel;
        private GameObject _levelGameObject;

        public static bool IsCurrentlySwitching;
        
        // Unity functions
        private void Awake() {
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
                App.Instance.sceneManager.LoadScene(SceneConstants.Credits);
                return;
            }
            StartCoroutine(LoadLevel(_currentLevel.nextLevel));
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