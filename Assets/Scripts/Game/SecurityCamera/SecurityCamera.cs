using System;
using System.Collections;

using Game.GameManagement;
using Game.NightLevels.Shooter;
using Game.PlayerComponents;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.SecurityCamera {
    public class SecurityCamera : MonoBehaviour {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject bulletPrefab;
        
        [SerializeField] private bool rotate = true;
        [SerializeField] private float rotateLeft = 45;
        [SerializeField] private float rotateRight = 45;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private Transform rotationPoint;
        
        [SerializeField] private float shootInterval = 1f;
        [SerializeField] private Light2D shootingSignalLight;
        [SerializeField] private SpriteRenderer shootingSignal;
        [SerializeField] private Color shootingColor = Color.red;
        [SerializeField] private Color idleColor = Color.green;
        
        private Vector2 DirectionToPlayer => (Player.Instance.transform.position - transform.position).normalized;
        private float _baseRotation;
        private float _adjustedRotationLeft;
        private float _adjustedRotationRight;
        
        // Unity functions
        private void Start() {
            shootingSignal.color = idleColor;
            shootingSignalLight.color = idleColor;
            _baseRotation = rotationPoint.rotation.eulerAngles.z;
            _adjustedRotationLeft = (_baseRotation + rotateLeft) % 360;
            _adjustedRotationRight = (_baseRotation - rotateRight + 360) % 360;
            
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }

        // Private functions
        private void OnLevelStart() {
            if (rotate) {
                StartCoroutine(Rotate());
            }
        }
        
        private IEnumerator ShootPlayer() {
            while (true) {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Shoot(DirectionToPlayer, gameObject);
                yield return new WaitForSeconds(shootInterval);
            }
        }
        
        private IEnumerator FacePlayer() {
            while (true) {
                Vector2 direction = DirectionToPlayer;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rotationPoint.rotation = Quaternion.Euler(0, 0, angle - 90);
                yield return new WaitForFixedUpdate();
            }
        }
        
        // Internal functions
        internal void StartShooting() {
            StopAllCoroutines();
            StartCoroutine(ShootPlayer());
            StartCoroutine(FacePlayer());
            shootingSignal.color = shootingColor;
            shootingSignalLight.color = shootingColor;
        }
        
        internal void StopShooting() {
            shootingSignal.color = idleColor;
            shootingSignalLight.color = idleColor;
            StopAllCoroutines();
            StartCoroutine(Rotate());
        }

        private IEnumerator Rotate() {
            while (true) {
                rotationPoint.Rotate(Vector3.forward, rotationSpeed);
                yield return new WaitForFixedUpdate();

                float currentAngle = rotationPoint.rotation.eulerAngles.z;
                if (currentAngle > _adjustedRotationLeft && rotationSpeed < 0) {
                    rotationSpeed = -rotationSpeed;
                } else if (currentAngle < _adjustedRotationRight && rotationSpeed > 0) {
                    rotationSpeed = -rotationSpeed;
                }
            }
        }
    }
}