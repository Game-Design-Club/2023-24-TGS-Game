using System.Collections;
using System.Collections.Generic;

using AppCore;

using UnityEngine;

namespace Game.LevelManagement {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private Level firstLevel;
        [SerializeField] private Level customFirstLevel;
        public Level CurrentLevel { get; private set; }
        private GameObject _levelGameObject;

        private bool _currentlySwitching;
        
        // Unity functions
        private void Awake() {
            if (customFirstLevel != null) {
                firstLevel = customFirstLevel;
            }
        }

        // Public functions
        public void LoadFirstLevel() {
            StartCoroutine(LoadLevel(firstLevel));
        }
        
        public void LoadNextLevel() {
            if (CurrentLevel.nextLevel is null) {
                Debug.LogWarning($"Next level for '{CurrentLevel}' is not assigned");
                return;
            }
            StartCoroutine(LoadLevel(CurrentLevel.nextLevel));
        }
        
        public void RestartLevel() {
            StartCoroutine(LoadLevel(CurrentLevel));
        }
        
        public IEnumerator LoadLevel(Level level, bool fade = true) {
            if (_currentlySwitching) {
                Debug.LogWarning("Tried to load level in the middle of loading another level");
                yield break;
            }

            if (level is null) {
                Debug.LogWarning("Tried to load a null level");
                yield break;
            }
            
            App.Instance.fadeManager.FadeIn();
            _currentlySwitching = true;
            yield return new WaitForSeconds(App.Instance.fadeManager.transitionPeriod);
            _currentlySwitching = false;
            App.Instance.fadeManager.FadeOut();
            
            ChangeCurrentLevel(level);
        }

        private void ChangeCurrentLevel(Level level)
        {
            if (CurrentLevel is not null)
            {
                Destroy(_levelGameObject);
            }

            _levelGameObject = Instantiate(level.gameObject);
            CurrentLevel = level;
        }
    }
}