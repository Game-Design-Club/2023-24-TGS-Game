using UnityEngine;

namespace UI {
    public class RandomStartRotation : MonoBehaviour {
        [SerializeField] private bool rotateOnStart = true;
        
        // Unity functions
        private void Start() {
            if (rotateOnStart) {
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
        }
    }
}