using System.Collections;

using Game.NightLevels.Shooter;
using Game.PlayerComponents;

using UnityEngine;

namespace Game.CameraShooter {
    public class CameraShooter : MonoBehaviour {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootInterval = 1f;
        
        private Vector2 DirectionToPlayer => (Player.Instance.transform.position - transform.position).normalized;
        
        // Private functions
        private IEnumerator ShootPlayer() {
            while (true) {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Shoot(DirectionToPlayer, gameObject);
                yield return new WaitForSeconds(shootInterval);
            }
        }
        
        // Internal functions
        internal void StartShooting() {
            StartCoroutine(ShootPlayer());
        }
        
        internal void StopShooting() {
            StopAllCoroutines();
        }
    }
}