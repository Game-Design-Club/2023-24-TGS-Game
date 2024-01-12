using UnityEngine;

using AppCore.AudioManagement;
using AppCore.InputManagement;
using AppCore.SceneManagement;
using AppCore.FadeManagement;

using UnityEngine.EventSystems;

namespace AppCore {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager audioManager;
        [SerializeField] public InputManager inputManager;
        [SerializeField] public SceneManager sceneManager;
        [SerializeField] public FadeManager fadeManager;

        [SerializeField] private EventSystem eventSystem;
        
        private void Awake() {
            // Sets up singleton pattern
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
                if (eventSystem.firstSelectedGameObject != null) {
                    SetUISelected(eventSystem.firstSelectedGameObject);
                }
            }
        }

        private void SetUISelected(GameObject uiObject) {
            eventSystem.SetSelectedGameObject(uiObject);
        }
    }
}
