using System.Collections;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.Shooter {
    public class Shooter : MonoBehaviour { // Shoots bullets periodically
        [SerializeField] private Transform shootTransform;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootFrequency = 2f;
        [SerializeField] private float startDelay;

        [SerializeField] private bool startShooting = true;
        
        private bool _shooting = true;
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }

        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }

        // Private functions
        private IEnumerator SpawnBullets() {
            yield return new WaitForSeconds(startDelay);
            while (true) {
                yield return new WaitUntil(() => _shooting);
                GameObject bullet = Instantiate(bulletPrefab, shootTransform.position, Quaternion.identity, BulletHolder.BulletHolderTransform);
                bullet.GetComponent<Bullet>().Shoot(transform.right, gameObject);
                yield return new WaitForSeconds(shootFrequency);
            }
        }

        private void OnLevelOver() {
            StopAllCoroutines();
        }

        private void OnLevelStart() {
            _shooting = startShooting;
            StartCoroutine(SpawnBullets());
        }

        public void SwitchShooting() {
            _shooting = !_shooting;
        }
    }
}