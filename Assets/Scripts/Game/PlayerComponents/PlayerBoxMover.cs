using System;

using AppCore;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : MonoBehaviour{
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private PlayerMovement _playerMovement;

        private bool _isTouchingBox;
        private bool _isGrabbingBox;
        
        [SerializeField] private Rigidbody2D _boxRB;
        
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
                _isTouchingBox = true;
                _boxRB = other.GetComponentInParent<Rigidbody2D>();
            }
        } 
        
        private void OnTriggerExit2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                _isTouchingBox = false;
                _boxRB = null;
            }
        }

        // Private functions
        private void OnPlayerMoved(Vector2 rawMovement) {
            if (_isGrabbingBox) {
                _boxRB.position += rawMovement;
            }
        }
        
        private void OnInteract() {
            if (_isTouchingBox) {
                GrabBox();
            }
        }
        
        private void OnInteractCancel() {
            if (_isGrabbingBox) {
                ReleaseBox();
            }
        }
        
        [ContextMenu("Grab Box")]
        private void GrabBox() {
            _isGrabbingBox = true;
            _playerMovement.SetMovementSpeed(boxMoveSpeed);
        }
        
        [ContextMenu("Release Box")]
        private void ReleaseBox() {
            _isGrabbingBox = false;
            _playerMovement.ResetMovementSpeed();
        }
    }
}