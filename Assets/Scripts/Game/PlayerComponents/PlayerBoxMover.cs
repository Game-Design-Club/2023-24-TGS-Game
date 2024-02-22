using System.Collections;
using System.Collections.Generic;

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
        internal Box BoxBox;
        internal Vector2 AttachDirection = Vector2.zero;

        public static List<Rigidbody2D> BoxChain = new();
        
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
                BoxBox = other.GetComponentInParent<Box>();
                BoxBox.EnteredTrigger();
                
                BoxObject = BoxRb.gameObject;
                
                AttachDirection = other.GetComponent<BoxTrigger>().AttachDirection;
            }
        }
        
        private void OnTriggerExit2D (Collider2D other) {
            if (IsGrabbingBox && other.gameObject == BoxTriggerObject) {
                ReleaseBox();
            }
            if (other.CompareTag(TagConstants.Box)) {
                if (!IsTouchingBox || other.gameObject != BoxTriggerObject) return;
                BoxBox.ExitedTrigger();
                
                BoxTriggerObject = null;
                IsTouchingBox = false;
                BoxRb = null;
                BoxBox = null;
                AttachDirection = Vector2.zero;
                BoxObject = null;
            }
        }

        // Private functions
        private void OnPlayerMoved(Vector2 rawMovement) {
            if (IsGrabbingBox) {
                foreach (Rigidbody2D box in BoxChain) {
                    box.position += rawMovement;
                }
                BoxChain.Clear();
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
            BoxBox.GrabbedBox();
        }
        
        private void ReleaseBox() {
            IsGrabbingBox = false;
            _playerMovement.ResetMovementSpeed();
            _playerMovement._boxAttachDirection = Vector2.zero;
            BoxBox.ReleasedBox();
        }
        
        // Internal functions
        internal Vector2 GetLockedMovement(Vector2 currentInput) {
            Vector2 movement = new(currentInput.x, currentInput.y);
            if (IsGrabbingBox) {
                if (AttachDirection.x != 0) {
                    // Moving along x axis
                    movement.y = 0;
                }
                if (AttachDirection.y != 0) {
                    // Moving along y axis
                    movement.x = 0;
                }
            }
            return movement.normalized;
        }
    }
}