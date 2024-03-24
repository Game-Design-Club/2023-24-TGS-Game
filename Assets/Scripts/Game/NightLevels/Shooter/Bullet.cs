using Game.GameManagement;
using Game.ParticleManagement;

using UnityEngine;

namespace Game.NightLevels.Shooter {
    public class Bullet : MonoBehaviour { // Bullet for the shooter
        [SerializeField] private float shootSpeed = 3f;
        [SerializeField] private ParticleSystem impactParticles;
        
        private GameObject _ignoreObject;
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // check if ignoreObject is in any of the parents of the collider (not just the object itself)
            if (other.gameObject == _ignoreObject || other.transform.IsChildOf(_ignoreObject.transform)) return;
            
            DestroyBullet();
        }

        // Private functions
        private void OnLevelStart() {
            DestroyBullet();
        }
        
        private void DestroyBullet() {
            Destroy(gameObject);
            if (impactParticles != null) {
                ParticleManager.PlayParticleEffect(impactParticles, transform.position);
            }
        }
        
        // Public functions
        public void Shoot(Vector2 direction, GameObject ignoreObject = null) {
            _ignoreObject = ignoreObject;
            GetComponent<Rigidbody2D>().velocity = direction * shootSpeed;
        }
    }
}