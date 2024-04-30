using Tools.Constants;

using UnityEngine;

namespace Game.SecurityCamera {
    public class SecurityCameraExitTrigger : MonoBehaviour {
        [SerializeField] private SecurityCamera securityCamera;
        
        // Unity functions
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                securityCamera.StopShooting();
            }
        }
    }
}