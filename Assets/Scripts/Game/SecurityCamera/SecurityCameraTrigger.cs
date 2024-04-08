using Tools.Constants;

using UnityEngine;

namespace Game.SecurityCamera {
    public class SecurityCameraTrigger : MonoBehaviour {
        [SerializeField] private SecurityCamera securityCamera;
        
        // Unity functions
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                securityCamera.StartShooting();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                securityCamera.StopShooting();
            }
        }
    }
}