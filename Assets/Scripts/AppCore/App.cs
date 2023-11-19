using UnityEngine;

namespace AppCore {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager.AudioManager AudioManager;
        [SerializeField] public InputManager.InputManager InputManager;
        [SerializeField] public SceneManager.SceneManager SceneManager;
        [SerializeField] public TimeManager.TimeManager TimeManager;
        
        private void Awake() {
            // Sets up singleton pattern
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
