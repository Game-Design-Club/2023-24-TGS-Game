using UnityEngine;
using App.Audio;
using App.Input;
using App.Scene;
using App.Time;

namespace App {
    public class App : MonoBehaviour {
        public static App Instance { get; private set; }

        [SerializeField] public AudioManager AudioManager;
        [SerializeField] public InputManager InputManager;
        [SerializeField] public SceneManager SceneManager;
        [SerializeField] public TimeManager TimeManager;
        
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
