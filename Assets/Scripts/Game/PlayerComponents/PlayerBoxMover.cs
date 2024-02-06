using System;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : MonoBehaviour{
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private PlayerMovement _playerMovement;

        private bool _isTouchingBox;
        private bool _isGrabbingBox;
        
        [SerializeField] private Transform _boxTransform;
        
        // Unity functions
        private void OnEnable() {
            _playerMovement.OnPlayerMoved += OnPlayerMoved;
        }

        private void OnDisable() {
            _playerMovement.OnPlayerMoved -= OnPlayerMoved;
        }

        private void Awake() {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                _isTouchingBox = true;
                _boxTransform = other.transform;
            }
        } 
        
        private void OnTriggerExit2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                _isTouchingBox = false;
                _boxTransform = null;
            }
        }

        // Private functions
        private void OnPlayerMoved(Vector2 rawMovement) {
            if (_isGrabbingBox) {
                _boxTransform.position += rawMovement;
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