using System;

using AppCore;

using Game.GameManagement;

using Tools.Helpfuls;

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
        
        internal event Action<Vector2> OnPlayerMoved;
        
        internal Axis _axisLock;
        
        // Unity functions
        private void OnEnable() {
            App.Instance.inputManager.OnMovement += OnMovement;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnMovement -= OnMovement;
        }

        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
            switch (_axisLock) {
                case Axis.Horizontal:
                    currentMovement.y = 0;
                    break;
                case Axis.Vertical:
                    currentMovement.x = 0;
                    break;
            }
            
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
    
            // Define the size of the player for BoxCast
            Vector2 localScale = transform.localScale;
            Vector2 size = new Vector2(localScale.x, localScale.y);
    
            // Perform BoxCast checks only if there is movement in that direction
            if (Mathf.Abs(_currentMovementInput.x) > 0) {
                RaycastHit2D hitX = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, new Vector2(_currentMovementInput.x, 0), Mathf.Abs(movement.x), wallLayer);
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