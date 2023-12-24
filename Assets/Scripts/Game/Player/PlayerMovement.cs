using UnityEngine;

namespace Game.Player {
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private float movementSpeed = 5f;
        
        private Vector2 _currentMovement;
        private Rigidbody2D _rigidbody2D;
        
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.position += _currentMovement * (movementSpeed * Time.deltaTime);
        }
        
        public void MovementChanged(Vector2 movementInput) {
            _currentMovement = movementInput;
        }
    }
}