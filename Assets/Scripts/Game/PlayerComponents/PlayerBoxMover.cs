using System;

using AppCore;

using Game.NightLevels.Box;

using Tools.Constants;
using Tools.Helpfuls;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : MonoBehaviour{
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private PlayerMovement _playerMovement;

        private bool _isTouchingBox;
        private bool _isGrabbingBox;
        
        private Rigidbody2D _boxRB;
        private Axis _axisLock;
        
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
                Debug.Log("Touching box");
                _isTouchingBox = true;
                _boxRB = other.GetComponentInParent<Rigidbody2D>();
                _axisLock = other.GetComponent<BoxTrigger>().axis;
            }
        } 
        
        private void OnTriggerExit2D (Collider2D other) {
            if (other.CompareTag(TagConstants.Box)) {
                Debug.Log("Not touching box");
                _isTouchingBox = false;
                _boxRB = null;
                _axisLock = Axis.None;
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
        
        private void GrabBox() {
            _isGrabbingBox = true;
            _playerMovement.SetMovementSpeed(boxMoveSpeed);
            _playerMovement._axisLock = _axisLock;
            Debug.Log("Grabbing box");
        }
        
        private void ReleaseBox() {
            _isGrabbingBox = false;
            _playerMovement.ResetMovementSpeed();
            _playerMovement._axisLock = Axis.None;
            Debug.Log("Releasing box");
        }
    }
}