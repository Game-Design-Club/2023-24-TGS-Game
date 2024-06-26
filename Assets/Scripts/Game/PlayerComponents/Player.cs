using UnityEngine;

namespace Game.PlayerComponents {
    public class Player : MonoBehaviour { // Player singleton
        public static Player Instance { get; private set; }
        
        // Unity functions
        private void Awake() {
            Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}