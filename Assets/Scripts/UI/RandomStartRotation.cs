using UnityEngine;

namespace UI {
    public class RandomStartRotation : MonoBehaviour { // Randomly rotates the object on start (used for UI nuts)
        [SerializeField] private bool rotateOnStart = true;
        
        // Unity functions
        private void Start() {
            if (rotateOnStart) {
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
        }
    }
}