using Game.GameManagement.PauseManagement;

using UnityEngine;

namespace Game.GameManagement.UIManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private GameObject hudCanvas;
        [SerializeField] private GameObject pauseCanvas;
        
        private void OnEnable() {
            PauseManagerEvents.OnGamePause += PauseGame;
            PauseManagerEvents.OnGameResume += ResumeGame;

            GameManagerEvents.OnLevelStart += OnLevelStart;
        }

        private void OnDisable() {
            PauseManagerEvents.OnGamePause -= PauseGame;
            PauseManagerEvents.OnGameResume -= ResumeGame;
            
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void PauseGame() {
            pauseCanvas.SetActive(true);
        }
        
        private void ResumeGame() {
            pauseCanvas.SetActive(false);
        }
        
        private void OnLevelStart() {
            // hudDisplay.SetActive(true);
            pauseCanvas.SetActive(false);
        }
    }
}