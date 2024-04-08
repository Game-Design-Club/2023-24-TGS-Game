using System;

using Tools.Constants;

using UnityEngine;

namespace Game.CameraShooter {
    public class CameraShooterTrigger : MonoBehaviour {
        [SerializeField] private CameraShooter cameraShooter;
        
        // Unity functions
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                cameraShooter.StartShooting();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                cameraShooter.StopShooting();
            }
        }
    }
}