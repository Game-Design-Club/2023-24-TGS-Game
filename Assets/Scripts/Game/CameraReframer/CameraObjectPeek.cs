using System.Collections;

using AppCore;
using AppCore.AudioManagement;

using Cinemachine;

using UnityEngine;
using UnityEngine.Events;

namespace Game.CameraReframer {
    public class CameraObjectPeek : MonoBehaviour {
        [SerializeField] private Transform objectToPeek;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private float waitTime = 1.5f;
        [SerializeField] private UnityEvent onMiddlePeek;
        [SerializeField] private SoundPackage middlePeekSound;
        [SerializeField] private bool freezePlayer = true;
        
        private CinemachineVirtualCamera _camera;
        
        private void Awake() {
            _camera = GetComponent<CinemachineVirtualCamera>();
            if (_camera == null) {
                Debug.LogWarning("CameraTrigger: No CinemachineVirtualCamera found on this object");
            }
        }

        private void Start() {
            _camera.Priority = -1;
        }

        // Public functions
        public void Peek() {
            StartCoroutine(PeekCoroutine());
        }
        
        // Private functions
        private IEnumerator PeekCoroutine() {
            Vector2 position = objectToPeek.position;
            _camera.ForceCameraPosition(position, Quaternion.identity);

            _camera.Priority = 50;
            
            if (freezePlayer) {
                App.InputManager.LockPlayerControls(this);
                App.InputManager.LockUI(this);
            }
            
            yield return new WaitForSeconds(moveTime);
            
            onMiddlePeek?.Invoke();
            App.AudioManager.PlaySFX(middlePeekSound);
            
            yield return new WaitForSeconds(waitTime);
            
            _camera.Priority = -1;

            yield return new WaitForSeconds(moveTime);

            if (freezePlayer) {
                App.InputManager.UnlockPlayerControls(this);
                App.InputManager.UnlockUI(this);
            }
        }
    }
}