using Constants;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.Shooter {
    public class Bullet : MonoBehaviour{
        [SerializeField] private float shootSpeed = 3f;
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            DestroyBullet();
        }

        // Private functions
        private void OnLevelStart() {
            DestroyBullet();
        }
        
        private void DestroyBullet() {
            Destroy(gameObject);
            // sfx, particle effects, etc.
        }
        
        // Public functions
        public void Shoot(Vector2 direction) {
            GetComponent<Rigidbody2D>().velocity = direction * shootSpeed;
        }
    }
}