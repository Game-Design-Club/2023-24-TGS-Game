using Cinemachine;

using Game.GameManagement;

using UnityEngine;

namespace Game.CameraReframer
{
    public class CameraReframer : MonoBehaviour {
        private CinemachineVirtualCamera _camera;
        private void Awake() {
            _camera = GetComponent<CinemachineVirtualCamera>();
            if (_camera == null) {
                Debug.LogWarning("CameraTrigger: No CinemachineVirtualCamera found on this object");
            }
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
    
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            _camera.Priority = 10;
        }
    
        private void OnTriggerExit2D(Collider2D other) {
            _camera.Priority = -1;
        }
    
        private void OnLevelStart() {
            _camera.Priority = -1;
        }
    }
}
