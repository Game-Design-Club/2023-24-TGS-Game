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
            GameManager.Instance.OnLevelOver += LevelOver;
            GameManager.Instance.OnLevelStart += LevelStart;
        }

        private void OnDisable() {
            GameManager.Instance.OnLevelOver -= LevelOver;
            GameManager.Instance.OnLevelStart -= LevelStart;
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

        private void LevelOver() {
            StopAllCoroutines();
        }

        private void LevelStart() {
            StartCoroutine(SpawnBullets());
        }
    }
}