using System.Collections;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.Shooter {
    public class Shooter : MonoBehaviour{
        [SerializeField] private Transform shootTransform;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootFrequency = 2f;
        
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
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator SpawnBullets() {
            while (true) {
                yield return new WaitForSeconds(shootFrequency);
                GameObject bullet = Instantiate(bulletPrefab, shootTransform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Shoot(transform.right);
            }
        }

        private void OnLevelOver() {
            StopAllCoroutines();
        }

        private void OnLevelStart() {
            StartCoroutine(SpawnBullets());
        }
    }
}