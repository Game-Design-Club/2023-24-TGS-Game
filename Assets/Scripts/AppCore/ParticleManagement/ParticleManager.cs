using Game.GameManagement;

using UnityEngine;

namespace AppCore.ParticleManagement {
    public class ParticleManager : MonoBehaviour { // Manages particle effects, so it's easier for other classes and automatically deletes on level restarting
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += EraseParticles;
            App.SceneManager.onSceneChange += EraseParticles;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= EraseParticles;
            App.SceneManager.onSceneChange -= EraseParticles;
        }
        
        // Private functions
        private void EraseParticles() {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }
        }
        
        // Public functions
        public void PlayParticleEffect(ParticleSystem particles, Transform t) {
            PlayParticleEffect(particles, transform.position, t.rotation);
        }
        
        public void PlayParticleEffect(ParticleSystem particles, Vector3 position) {
            PlayParticleEffect(particles, position, Quaternion.identity);
        }
        
        public void PlayParticleEffect(ParticleSystem particles, Vector3 position, Quaternion rotation) {
            Instantiate(particles, position, rotation, transform);
        }

        public void AddObject(GameObject obj) {
            obj.transform.parent = transform;
        }
    }
}