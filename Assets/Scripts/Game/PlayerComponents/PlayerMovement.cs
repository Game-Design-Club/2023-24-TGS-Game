using System;
using System.Collections.Generic;

using AppCore;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerMovement : MonoBehaviour {
        // Handles all player movement
        // Lots of fancy smooth movement and box pushing going on, enabled mostly by raycasts
        
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private bool smoothMovement = true;
        [SerializeField] private float snapDistance = 0.02f;
        [SerializeField] private float updatePosDistance = 0.01f;
        
        [SerializeField] private AudioClip hitWallSound;

        private Vector2 _currentMovement;
        private float _currentMovementSpeed;
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider;
        private PlayerBoxMover _boxPusher;
        
        internal Vector2 CurrentMovementInput { get; private set; }
        
        internal event Action<Vector2> OnPlayerMoved;
        
        private bool _lastFrameHitWall = false;
        
        // Unity functions
        private void OnEnable() {
            App.Instance.inputManager.OnMovement += OnMovement;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnMovement -= OnMovement;
        }

        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxPusher = GetComponent<PlayerBoxMover>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Start() {
            _currentMovementSpeed = movementSpeed;
        }

        private void FixedUpdate() {
            MovePlayer();
        }

        // Private functions
        private void OnMovement(Vector2 movementInput) {
            CurrentMovementInput = movementInput;
            CurrentMovementInput.Normalize();
        }

        private void MovePlayer() {
            _currentMovement = _boxPusher.GetLockedMovement(CurrentMovementInput);
            
            float movementDistance = _currentMovementSpeed * Time.deltaTime;
            Vector2 originalMovement = _currentMovement * movementDistance;
            Vector2 newPosition = _rigidbody2D.position + originalMovement;
            
            Vector2 actualMovement = originalMovement;
            
            if (smoothMovement) {
                newPosition = SmoothMovement(originalMovement);
                actualMovement = newPosition - _rigidbody2D.position;
            }
            
            _rigidbody2D.position = newPosition;
            
            OnPlayerMoved?.Invoke(actualMovement);
        }


        private Vector2 SmoothMovement(Vector2 movement) {
            Vector2 newPosition = _rigidbody2D.position + movement;
    
            Vector2 size = _boxCollider.size * transform.localScale;
            
            bool hitWall = false;
            
            List<Collider2D> thisFrameColliders = new List<Collider2D>();
    
            if (Mathf.Abs(_currentMovement.x) > 0) {
                
                RaycastHit2D[] playerHits = new RaycastHit2D[4];
                Physics2D.BoxCastNonAlloc(_rigidbody2D.position, size, 0f, new Vector2(_currentMovement.x, 0), playerHits,Mathf.Abs(movement.x), wallLayer);
                
                RaycastHit2D hitX = new RaycastHit2D();
                foreach (RaycastHit2D hit in playerHits) {
                    // Check if touching box
                    if (hit.collider == null) continue;

                    if (!_boxPusher.IsGrabbingBox ||
                        (_boxPusher.IsGrabbingBox && hit.collider.gameObject != _boxPusher.BoxObject)) {
                        hitX = hit;
                    }
                }
                
                if (_boxPusher.IsGrabbingBox) {
                    RaycastHit2D boxRaycast = _boxPusher.BoxBox.SendBoxCast(new Vector2(_currentMovement.x, 0), Mathf.Abs(movement.x), wallLayer);
                    if (boxRaycast.collider != null && boxRaycast.collider.gameObject != _boxPusher.BoxObject) {
                        hitX = boxRaycast;
                    }
                }
                
                if (hitX.collider != null) {
                    hitWall = true;
                    if (hitX.distance > updatePosDistance) {
                        newPosition.x = _rigidbody2D.position.x + _currentMovement.x * (hitX.distance - snapDistance);
                    } else {
                        newPosition.x = _rigidbody2D.position.x;
                    }
                }
            }

            if (Mathf.Abs(_currentMovement.y) > 0) {
                RaycastHit2D[] playerHits = new RaycastHit2D[4];
                Physics2D.BoxCastNonAlloc(_rigidbody2D.position, size, 0f, new Vector2(0, _currentMovement.y), playerHits,Mathf.Abs(movement.y), wallLayer);
                
                RaycastHit2D hitY = new RaycastHit2D();
                foreach (RaycastHit2D hit in playerHits) {
                    if (hit.collider == null) continue;

                    if (!_boxPusher.IsGrabbingBox ||
                        (_boxPusher.IsGrabbingBox && hit.collider.gameObject != _boxPusher.BoxObject)) {
                        hitY = hit;
                    }
                }
                
                if (_boxPusher.IsGrabbingBox) {
                    RaycastHit2D boxRaycast = _boxPusher.BoxBox.SendBoxCast(new Vector2(0, _currentMovement.y), Mathf.Abs(movement.y), wallLayer);
                    if (boxRaycast.collider != null && boxRaycast.collider.gameObject != _boxPusher.BoxObject) {
                        hitY = boxRaycast;
                    }
                }
                
                if (hitY.collider != null) {
                    hitWall = true;
                    if (hitY.distance > updatePosDistance) {
                        newPosition.y = _rigidbody2D.position.y + _currentMovement.y * (hitY.distance - snapDistance);
                    } else {
                        newPosition.y = _rigidbody2D.position.y;
                    }
                }
            }
            
            if (hitWall && !_lastFrameHitWall) {
                App.Instance.audioManager.PlaySFX(hitWallSound);
            }
            
            _lastFrameHitWall = hitWall;
            
            return newPosition;
        }

        
        // Protected functions
        internal void SetMovementSpeed(float speed) {
            _currentMovementSpeed = speed;
        }
        
        internal void ResetMovementSpeed() {
            SetMovementSpeed(movementSpeed);
        }
    }
}