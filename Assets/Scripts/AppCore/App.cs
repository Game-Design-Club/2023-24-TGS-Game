using UnityEngine;
using AppCore;
using AppCore.AudioManagement;
using AppCore.InputManagement;
using AppCore.TimeManagement;
using AppCore.SceneManagement;
using AppCore.FadeManagement;

namespace AppCore {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager audioManager;
        [SerializeField] public InputManager inputManager;
        [SerializeField] public SceneManager sceneManager;
        [SerializeField] public TimeManager timeManager;
        [SerializeField] public FadeManager fadeManager;
        
        private void Awake() {
            // Sets up singleton pattern
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
