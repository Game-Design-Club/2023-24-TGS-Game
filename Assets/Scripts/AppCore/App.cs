using UnityEngine;

using AppCore.AudioManagement;
using AppCore.DialogueManagement;
using AppCore.Data_Management;
using AppCore.InputManagement;
using AppCore.ParticleManagement;
using AppCore.SceneManagement;
using AppCore.SpeedrunTimer;
using AppCore.TransitionManagement;

using UnityEngine.Serialization;

namespace AppCore {
    public class App : MonoBehaviour {
        // Singleton class for the app
        // Contains references to all managers that can be used in different scenes in the game
        // (for example main menu, game, even credits, etc.)
        // Consolidates singleton patterns into one place so there aren't instance checks in every script
        private static App s_instance;
        
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private SceneManager sceneManager;
        [FormerlySerializedAs("fadeManager")] [SerializeField] private TransitionManager transitionManager;
        [SerializeField] private PlayerDataManager playerDataManager;
        [FormerlySerializedAs("cutsceneManager")] [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private ParticleManager particleManager;
        [SerializeField] private TimerManager timerManager;

        public static AudioManager AudioManager => s_instance.audioManager;
        public static InputManager InputManager => s_instance.inputManager;
        public static SceneManager SceneManager => s_instance.sceneManager;
        public static TransitionManager TransitionManager => s_instance.transitionManager;
        public static PlayerDataManager PlayerDataManager => s_instance.playerDataManager;
        public static ParticleManager ParticleManager => s_instance.particleManager;
        public static DialogueManager DialogueManager => s_instance.dialogueManager;
        public static TimerManager TimerManager => s_instance.timerManager;
        
        private void Awake() {
            // Sets up singleton pattern
            if (s_instance == null) {
                s_instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
        
        // Used to check if the App instance is in the scene, otherwise throw an error
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CheckAppInstance() {
            if (s_instance == null) {
                Debug.LogError("No App instance found in the scene.");
            }
        }
    }
}
