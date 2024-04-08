using System.Collections;

using Game.NightLevels.Shooter;
using Game.PlayerComponents;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.SecurityCamera {
    public class SecurityCamera : MonoBehaviour {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootInterval = 1f;
        [SerializeField] private Light2D shootingSignalLight;
        [SerializeField] private SpriteRenderer shootingSignal;
        [SerializeField] private Color shootingColor = Color.red;
        [SerializeField] private Color idleColor = Color.green;
        
        private Vector2 DirectionToPlayer => (Player.Instance.transform.position - transform.position).normalized;
        
        // Unity functions
        private void Start() {
            shootingSignal.color = idleColor;
            shootingSignalLight.color = idleColor;
        }
        
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
            shootingSignal.color = shootingColor;
            shootingSignalLight.color = shootingColor;
        }
        
        internal void StopShooting() {
            StopAllCoroutines();
            shootingSignal.color = idleColor;
            shootingSignalLight.color = idleColor;
        }
    }
}