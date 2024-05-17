using System;
using System.Collections;
using System.Collections.Generic;

using AppCore;

using Game.GameManagement;
using Game.Interactables;
using Game.NightLevels.Box;
using Tools.Constants;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.PlayerComponents {
    public class PlayerBoxMover : MonoBehaviour {
        // Lets player move boxes, muy complicado
        // Automatically handles touching multiple boxes
        
        [SerializeField] private float boxMoveSpeed = 5f;
        
        private PlayerMovement _playerMovement;

        internal bool IsGrabbingBox;

        internal List<BoxTrigger> BoxTriggers  = new();
        internal GameObject BoxTriggerObject => BoxTriggers.Count > 0 ? BoxTriggers[0].gameObject : null;
        internal Box BoxBox => BoxTriggerObject?.GetComponentInParent<Box>(); //"Box" script that is attached to Box Object
        internal Rigidbody2D BoxRb => BoxBox?.GetComponent<Rigidbody2D>();
        internal GameObject BoxObject => BoxRb?.gameObject;
        internal Vector2 AttachDirection => BoxTriggers.Count > 0 ? BoxTriggers[0].AttachDirection : Vector2.zero;

        public static List<Rigidbody2D> BoxChain = new();
        
        public Action<bool> onBoxGrabbed;
        
        // Unity functions
        private void OnEnable() {
            _playerMovement.OnPlayerMoved += OnPlayerMoved;
            App.InputManager.OnInteract += OnInteract;
            App.InputManager.OnInteractCancel += OnInteractCancel;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }

        private void OnDisable() {
            _playerMovement.OnPlayerMoved -= OnPlayerMoved;
            App.InputManager.OnInteract -= OnInteract;
            App.InputManager.OnInteractCancel -= OnInteractCancel;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }

        private void Awake() {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter2D (Collider2D other) {
            if (!other.CompareTag(TagConstants.Box)) return;

            BoxTriggers.Add(other.GetComponent<BoxTrigger>());
            
            if (BoxTriggers.Count == 1) {
                // First box touched
                BoxBox.EnteredTrigger();

                StartCoroutine(HandleTutorialPopup());
            }
        }
        
        private void OnTriggerExit2D (Collider2D other) {
            if (!other.CompareTag(TagConstants.Box)) return;
            
            if (other.GetComponent<BoxTrigger>() == BoxTriggers[0]) {
                BoxBox.ExitedTrigger();
            }
            
            BoxTriggers.Remove(other.GetComponent<BoxTrigger>());
            
            if (IsGrabbingBox && other.GetComponent<BoxTrigger>() == BoxTriggers[0]) {
                // Somehow exited the trigger while still grabbing the box (has happened before)
                ReleaseBox();
            }
            
            if (BoxTriggers.Count > 0) {
                // Was touching multiple boxes at the same time
                BoxBox.EnteredTrigger();
            }
        }

        // Private functions
        private void OnPlayerMoved(Vector2 rawMovement, bool extra = false) {
            if (IsGrabbingBox) {
                foreach (Rigidbody2D box in BoxChain) {
                    box.position += rawMovement;
                }

                if (BoxChain.Count == 0 && extra) {
                    BoxRb.position += rawMovement;
                }
                
                if (!extra) {
                    BoxChain.Clear();
                }
            }
        }
        
        private void OnInteract() {
            if (BoxTriggers.Count > 0 && !IsGrabbingBox) {
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
            BoxBox.GrabbedBox();
            onBoxGrabbed?.Invoke(true);
        }
        
        private void ReleaseBox() {
            IsGrabbingBox = false;
            _playerMovement.ResetMovementSpeed();
            BoxBox.ReleasedBox();
            onBoxGrabbed?.Invoke(false);
        }
        
        private void OnLevelOver() {
            if (IsGrabbingBox) {
                ReleaseBox();
            }
        }

        private IEnumerator HandleTutorialPopup() {
            if (App.PlayerDataManager.HasInteractedWithRobot) yield break;
            
            InteractionsPopup.Show("Hold space to push/pull");
            
            yield return new WaitUntil(() => 
                (IsGrabbingBox && _playerMovement.CurrentMovementInput != Vector2.zero));
            
            InteractionsPopup.Hide();
            App.PlayerDataManager.HasInteractedWithRobot = true;
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