using System;

using AppCore;

using Game.NightLevels.Box;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private bool smoothMovement = true;
        [SerializeField] private float snapDistance = 0.01f;

        private Vector2 _currentMovementInput;
        private float _currentMovementSpeed;
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider;
        private PlayerBoxMover _boxPusher;
        
        internal event Action<Vector2> OnPlayerMoved;
        
        internal Vector2 _boxAttachDirection;
        
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

        private void Update() {
            MovePlayer();
        }

        // Private functions
        private void OnMovement(Vector2 movementInput) {
            _currentMovementInput = movementInput;
            _currentMovementInput.Normalize();
        }

        private void MovePlayer() {
            Vector2 currentMovement = _currentMovementInput;
            
            if (_boxAttachDirection.x != 0 && _boxAttachDirection.y == 0) {
                currentMovement.y = 0;
            } else if (_boxAttachDirection.y != 0 && _boxAttachDirection.x == 0) {
                currentMovement.x = 0;
            }
            
            currentMovement.Normalize();
            
            float movementDistance = _currentMovementSpeed * Time.deltaTime;
            Vector2 originalMovement = currentMovement * movementDistance;
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
    
            if (Mathf.Abs(_currentMovementInput.x) > 0) {
                
                RaycastHit2D hitX = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, new Vector2(_currentMovementInput.x, 0), Mathf.Abs(movement.x), wallLayer);
                
                if (_boxPusher.IsGrabbingBox && ((_boxAttachDirection.x < 0 && _currentMovementInput.x > 0) || (_boxAttachDirection.x > 0 && _currentMovementInput.x < 0))) {
                    hitX = _boxPusher.BoxBox.SendBoxCast(new Vector2(_currentMovementInput.x, 0), Mathf.Abs(movement.x), wallLayer);
                }
                
                if (hitX.collider != null) {
                    if (hitX.distance > snapDistance) {
                        newPosition.x = _rigidbody2D.position.x + _currentMovementInput.x * (hitX.distance - snapDistance);
                    } else {
                        newPosition.x = _rigidbody2D.position.x;
                    }
                }
            }

            if (Mathf.Abs(_currentMovementInput.y) > 0) {
                RaycastHit2D hitY = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, new Vector2(0, _currentMovementInput.y), Mathf.Abs(movement.y), wallLayer);
                
                if (_boxPusher.IsGrabbingBox && ((_boxAttachDirection.y < 0 && _currentMovementInput.y > 0) || (_boxAttachDirection.y > 0 && _currentMovementInput.y < 0))) {
                    hitY = _boxPusher.BoxBox.SendBoxCast(new Vector2(0, _currentMovementInput.y), Mathf.Abs(movement.y), wallLayer);
                }
                
                if (hitY.collider != null) {
                    if (hitY.distance > snapDistance) {
                        newPosition.y = _rigidbody2D.position.y + _currentMovementInput.y * (hitY.distance - snapDistance);
                    } else {
                        newPosition.y = _rigidbody2D.position.y;
                    }
                }
            }
    
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