using UnityEngine;

using AppCore.AudioManagement;
using AppCore.InputManagement;
using AppCore.SceneManagement;
using AppCore.TransitionManagement;

using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace AppCore {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager audioManager;
        [SerializeField] public InputManager inputManager;
        [SerializeField] public SceneManager sceneManager;
        [FormerlySerializedAs("fadeManager")] [SerializeField] public TransitionManager transitionManager;

        
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
