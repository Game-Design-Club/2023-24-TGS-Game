using System;

using Game.GameManagement.LevelManagement;

using UnityEngine;

namespace Game.GameManagement.UIManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private GameObject hudCanvas;
        [SerializeField] private GameObject pauseCanvas;
        
        private void OnEnable() {
            PauseManager.Instance.OnGamePause += PauseGame;
            PauseManager.Instance.OnGameResume += ResumeGame;

            GameManager.Instance.OnLevelStart += LevelStart;
        }

        private void OnDisable() {
            PauseManager.Instance.OnGamePause -= PauseGame;
            PauseManager.Instance.OnGameResume -= ResumeGame;
            
            GameManager.Instance.OnLevelStart -= LevelStart;
        }
        
        private void PauseGame() {
            pauseCanvas.SetActive(true);
        }
        
        private void ResumeGame() {
            pauseCanvas.SetActive(false);
        }
        
        private void LevelStart() {
            // hudDisplay.SetActive(true);
            pauseCanvas.SetActive(false);
        }
    }
}