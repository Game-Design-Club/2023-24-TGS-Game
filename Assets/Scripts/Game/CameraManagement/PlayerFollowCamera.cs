using Cinemachine;

using Game.GameManagement;

using UnityEngine;

namespace Game.CameraManagement
{
    public class PlayerFollowCamera : MonoBehaviour {
        private CinemachineVirtualCamera _virtualCamera;
    
        private void Awake() {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }
    
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }
    
        private void OnLevelStart() {
            Transform playerTransform = PlayerComponents.Player.Instance.transform; // (Player.Player) because of namespace conflict
            _virtualCamera.Follow = playerTransform;
            _virtualCamera.ForceCameraPosition(playerTransform.position, transform.rotation);
        }
    
        private void OnLevelOver() {
            _virtualCamera.Follow = null;
        }
    }
}
