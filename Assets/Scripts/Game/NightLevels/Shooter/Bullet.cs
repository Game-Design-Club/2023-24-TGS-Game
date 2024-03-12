using AppCore;

using Game.GameManagement;
using Game.ParticleManagement;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NightLevels.Shooter {
    public class Bullet : MonoBehaviour {
        [SerializeField] private float shootSpeed = 3f;
        [SerializeField] private ParticleSystem impactParticles;
        
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip impactSound;
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            DestroyBullet();
            if (impactSound != null) {
                App.Instance.audioManager.sfx.Play(impactSound);
            }
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
        public void Shoot(Vector2 direction) {
            GetComponent<Rigidbody2D>().velocity = direction * shootSpeed;
            if (shootSound != null) {
                App.Instance.audioManager.sfx.Play(shootSound);
            }
        }
    }
}