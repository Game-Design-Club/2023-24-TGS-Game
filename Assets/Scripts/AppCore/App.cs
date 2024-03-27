using UnityEngine;

using AppCore.AudioManagement;
using AppCore.CutsceneManagement;
using AppCore.Data_Management;
using AppCore.InputManagement;
using AppCore.SceneManagement;
using AppCore.TransitionManagement;

using UnityEditor;

using UnityEngine.Serialization;

namespace AppCore {
    public class App : MonoBehaviour {
        // Singleton class for the app
        // Contains references to all managers that can be used in different scenes in the game
        // (for example main menu, game, even credits, etc.)
        // Consolidates singleton patterns into one place so there aren't instance checks in every script
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager audioManager;
        [SerializeField] public InputManager inputManager;
        [SerializeField] public SceneManager sceneManager;
        [FormerlySerializedAs("fadeManager")] [SerializeField] public TransitionManager transitionManager;
        [SerializeField] public PlayerDataManager playerDataManager;
        [SerializeField] public CutsceneManager cutsceneManager;

        
        private void Awake() {
            // Sets up singleton pattern
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
        
        // Used to check if the App instance is in the scene, otherwise throw an error
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CheckAppInstance() {
            if (Instance == null) {
                Debug.LogError("No App instance found in the scene.");
            }
        }
    }
}
