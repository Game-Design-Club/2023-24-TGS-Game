using AppCore;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerMovement : MonoBehaviour{
        [SerializeField] private float movementSpeed = 5f;
        
        internal Vector2 CurrentMovement;
        private Rigidbody2D _rigidbody2D;
        
        // Unity functions
        private void OnEnable() {
            App.Instance.inputManager.OnMovement += MovementChanged;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnMovement -= MovementChanged;
        }

        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.position += CurrentMovement * (movementSpeed * Time.deltaTime);
        }
        
        // Public functions
        public virtual void MovementChanged(Vector2 movementInput) {
            CurrentMovement = movementInput;
            CurrentMovement.Normalize();
        }
    }
}