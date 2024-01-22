using AppCore;
using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private bool smoothMovement = true;

        protected Vector2 _currentMovement;
        protected Rigidbody2D _rigidbody2D;

        private const float _wallDistance = 0.01f;

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

        private void Update() {
            MovePlayer();
        }

        // Private functions
        private void OnMovement(Vector2 movementInput) {
            _currentMovement = movementInput;
            _currentMovement.Normalize();
        }

        // Protected functions
        protected virtual void MovePlayer() {
            float movementDistance = movementSpeed * Time.deltaTime;
            Vector2 movement = _currentMovement * movementDistance;
            Vector2 newPosition = _rigidbody2D.position + movement;

            if (smoothMovement) {
                newPosition = SmoothMovement(movement);
            }

            _rigidbody2D.position = newPosition;
        }


        private Vector2 SmoothMovement(Vector2 movement) {
            Vector2 newPosition = _rigidbody2D.position + movement;
            
            // Define the size of the player for BoxCast
            Vector2 localScale = transform.localScale;
            Vector2 size = new (localScale.x, localScale.y);
        
            // Separate BoxCast checks for X and Y axes
            if (_currentMovement.x != 0) {
                RaycastHit2D hitX = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, new Vector2(_currentMovement.x, 0), Mathf.Abs(movement.x), wallLayer);
                if (hitX.collider != null) {
                    if (hitX.distance > _wallDistance) {
                        newPosition.x = _rigidbody2D.position.x + _currentMovement.x * (hitX.distance - _wallDistance);
                    } else {
                        newPosition.x = _rigidbody2D.position.x;
                    }
                }
            }

            if (_currentMovement.y != 0) {
                RaycastHit2D hitY = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, new Vector2(0, _currentMovement.y), Mathf.Abs(movement.y), wallLayer);
                if (hitY.collider != null) {
                    if (hitY.distance > _wallDistance) {
                        newPosition.y = _rigidbody2D.position.y + _currentMovement.y * (hitY.distance - _wallDistance);
                    } else {
                        newPosition.y = _rigidbody2D.position.y;
                    }
                }
            }

            return newPosition;
        }
    }
}