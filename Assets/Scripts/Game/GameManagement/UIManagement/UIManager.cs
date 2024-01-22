using AppCore;

using Game.GameManagement.PauseManagement;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.GameManagement.UIManagement {
    public class UIManager : MonoBehaviour {
        [SerializeField] private PauseManager pauseManager;
        
        [SerializeField] private GameObject hudCanvas;
        [SerializeField] private GameObject pauseCanvas;

        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject defaultSelectedGameObjectPaused;
        
        private void OnEnable() {
            PauseManagerEvents.OnGamePause += OnGamePause;
            PauseManagerEvents.OnGameResume += OnGameResume;
            
            App.Instance.inputManager.OnCancel += OnCancelPressed;

            GameManagerEvents.OnLevelStart += OnLevelStart;
        }

        private void OnDisable() {
            PauseManagerEvents.OnGamePause -= OnGamePause;
            PauseManagerEvents.OnGameResume -= OnGameResume;

            App.Instance.inputManager.OnCancel += OnCancelPressed;
            
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnGamePause() {
            pauseCanvas.SetActive(true);
            if (defaultSelectedGameObjectPaused != null) eventSystem.SetSelectedGameObject(defaultSelectedGameObjectPaused);
        }
        
        private void OnGameResume() {
            pauseCanvas.SetActive(false);
        }
        
        private void OnLevelStart() {
            // hudDisplay.SetActive(true);
            pauseCanvas.SetActive(false);
        }
        
        private void OnCancelPressed() {
            pauseManager.TogglePaused();
        }
    }
}