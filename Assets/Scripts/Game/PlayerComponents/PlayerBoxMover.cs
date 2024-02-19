using AppCore;
using Game.NightLevels.Box;
using Tools.Constants;
using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : MonoBehaviour {
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private PlayerMovement _playerMovement;

        internal bool IsTouchingBox;
        internal bool IsGrabbingBox;

        internal GameObject BoxTriggerObject;
        internal GameObject BoxObject;
        internal Rigidbody2D BoxRb;
        internal Vector2 AttachDirection = Vector2.zero;
        
        // Unity functions
        private void OnEnable() {
            _playerMovement.OnPlayerMoved += OnPlayerMoved;
            App.Instance.inputManager.OnInteract += OnInteract;
            App.Instance.inputManager.OnInteractCancel += OnInteractCancel;
        }

        private void OnDisable() {
            _playerMovement.OnPlayerMoved -= OnPlayerMoved;
            App.Instance.inputManager.OnInteract -= OnInteract;
            App.Instance.inputManager.OnInteractCancel -= OnInteractCancel;
        }

        private void Awake() {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                if (IsTouchingBox) return;
                IsTouchingBox = true;
                BoxTriggerObject = other.gameObject;
                BoxRb = other.GetComponentInParent<Rigidbody2D>();
                BoxObject = BoxRb.gameObject;
                
                AttachDirection = other.GetComponent<BoxTrigger>().AttachDirection;
            }
        } 
        
        private void OnTriggerExit2D (Collider2D other) {
            if (IsGrabbingBox) {
                ReleaseBox();
            }
            if (other.CompareTag(TagConstants.Box)) {
                if (!IsTouchingBox || other.gameObject != BoxTriggerObject) return;
                BoxTriggerObject = null;
                IsTouchingBox = false;
                BoxRb = null;
                AttachDirection = Vector2.zero;
                BoxObject = null;
            }
        }

        // Private functions
        private void OnPlayerMoved(Vector2 rawMovement) {
            if (IsGrabbingBox) {
                BoxRb.position += rawMovement;
            }
        }
        
        private void OnInteract() {
            if (IsTouchingBox) {
                GrabBox();
            }
        }
        
        private void OnInteractCancel() {
            if (IsGrabbingBox) {
                ReleaseBox();
            }
        }
        
        private void GrabBox() {
            IsGrabbingBox = true;
            _playerMovement.SetMovementSpeed(boxMoveSpeed);
            _playerMovement._boxAttachDirection = AttachDirection;
        }
        
        private void ReleaseBox() {
            IsGrabbingBox = false;
            _playerMovement.ResetMovementSpeed();
            _playerMovement._boxAttachDirection = Vector2.zero;
        }
    }
}