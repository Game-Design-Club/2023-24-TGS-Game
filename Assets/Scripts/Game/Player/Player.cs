using System;

using UnityEngine;

namespace Game.Player {
    public class Player : MonoBehaviour{
        public static Player Instance { get; private set; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate Player found and deleted.");
            } else {
                Instance = this;
            }
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}