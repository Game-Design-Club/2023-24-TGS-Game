using UnityEngine;

namespace Game.PlayerComponents {
    public class Player : MonoBehaviour{
        public static Player Instance { get; private set; }

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