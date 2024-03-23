using UnityEngine;

using AppCore.AudioManagement;
using AppCore.Data_Management;
using AppCore.InputManagement;
using AppCore.SceneManagement;
using AppCore.TransitionManagement;

using UnityEditor;

using UnityEngine.Serialization;

namespace AppCore {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager audioManager;
        [SerializeField] public InputManager inputManager;
        [SerializeField] public SceneManager sceneManager;
        [FormerlySerializedAs("fadeManager")] [SerializeField] public TransitionManager transitionManager;
        [SerializeField] public PlayerDataManager playerDataManager;

        
        private void Awake() {
            // Sets up singleton pattern
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CheckAppInstance() {
            if (Instance == null) {
                Debug.LogError("No App instance found in the scene.");
            }
        }
    }
}
