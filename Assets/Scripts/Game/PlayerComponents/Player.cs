using UnityEngine;

namespace Game.PlayerComponents {
    public class Player : MonoBehaviour { // Player singleton
        public static Player Instance { get; private set; }
        
        // Unity functions
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.LogWarning("Multiple Player instances detected");
            }
            
            
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}