using System.Collections;

using AppCore;
using AppCore.TransitionManagement;

using Tools.Constants;

using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    public class LevelManager : MonoBehaviour { // Manages the levels in the game, loading them and keeping track of the current level
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
        
        public void LoadSavedLevel() { // Loads the last completed level from player data
            if (levelsData == null) {
                Debug.LogError("No levels data assigned to the level manager");
                return;
            }

            Level levelToLoad = levelsData[App.PlayerDataManager.LastCompletedLevelIndex];
            _currentLevelIndex = App.PlayerDataManager.LastCompletedLevelIndex;
            if (customFirstLevel != null) levelToLoad = customFirstLevel;
            StartCoroutine(LoadLevel(levelToLoad, false));
        }
        
        public void LoadNextLevel() {
            _currentLevelIndex++;
            if (levelsData[_currentLevelIndex] == null) {
                // If there are no more levels, go to the credits
                _currentLevelIndex = 0;
                App.PlayerDataManager.LastLevelCompleted(_currentLevelIndex);
                App.SceneManager.LoadScene(SceneConstants.ClosingDialogue);
                return;
            }
            StartCoroutine(LoadLevel(levelsData[_currentLevelIndex]));
            App.PlayerDataManager.LastLevelCompleted(_currentLevelIndex);
        }
        
        public void RestartLevel() {
            StartCoroutine(LoadLevel(_currentLevel));
            if (_currentLevelIndex == 0) {
                App.TimerManager.ResetTimer();
            }
        }
        
        public void LoadLevel(int levelIndex) {
            if (levelsData[levelIndex] == null) {
                Debug.LogWarning($"Tried to load level {levelIndex} but it doesn't exist");
                return;
            }
            StartCoroutine(LoadLevel(levelsData[levelIndex]));
            _currentLevelIndex = levelIndex;
        }
        
        public IEnumerator LoadLevel(Level level, bool fade = true) { // default fade is true
            if (IsCurrentlySwitching) {
                Debug.LogWarning("Tried to load level in the middle of loading another level");
                yield break;
            }

            if (level == null) {
                Debug.LogWarning("Tried to load a null level");
                yield break;
            }
            if (fade) {
                App.TransitionManager.FadeIn(TransitionType.Wipe);
                IsCurrentlySwitching = true;
                yield return new WaitForSecondsRealtime(App.TransitionManager.wipeTime);
                IsCurrentlySwitching = false;
                App.TransitionManager.FadeOut(TransitionType.Wipe);
            }
            ChangeCurrentLevel(level);
        }

        private void ChangeCurrentLevel(Level level) { // Physically changes the current level game objects
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