using System;

using Game.GameManagement;

using UnityEngine;

namespace Game.ParticleManagement {
    public class ParticleManager : MonoBehaviour {
        [SerializeField] private Transform particleParent;
        
        private static ParticleManager _instance;
        
        // Unity functions
        private void Awake() {
            if (_instance == null) {
                _instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        // Private functions
        private void OnLevelStart() {
            foreach (Transform t in particleParent) {
                Destroy(t.gameObject);
            }
        }
        
        // Public functions
        public static void PlayParticleEffect(ParticleSystem particleSystem, Transform transform) {
            PlayParticleEffect(particleSystem, transform.position, transform.rotation);
        }
        
        public static void PlayParticleEffect(ParticleSystem particleSystem, Vector3 position) {
            PlayParticleEffect(particleSystem, position, Quaternion.identity);
        }
        
        public static void PlayParticleEffect(ParticleSystem particleSystem, Vector3 position, Quaternion rotation) {
            Instantiate(particleSystem, position, rotation, _instance.particleParent);
        }
    }
}