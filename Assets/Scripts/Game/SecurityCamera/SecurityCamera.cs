using System.Collections;

using Game.GameManagement;
using Game.NightLevels.Shooter;
using Game.PlayerComponents;

using Tools.Constants;

using UnityEngine;

namespace Game.SecurityCamera {
    public class SecurityCamera : MonoBehaviour {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private GameObject bulletPrefab;
        
        [SerializeField] private bool rotate = true;
        [SerializeField] private bool rotateClockwiseFirst = true;
        [SerializeField] private float rotateCounterClockwise = 45;
        [SerializeField] private float rotateClockwise = 45;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float playerLockRotationSpeed = 2f;
        [SerializeField] private Transform rotationPoint;
        
        [SerializeField] private float shootInterval = 1f;
        
        private Vector2 DirectionToPlayer => (Player.Instance.transform.position - transform.position).normalized;
        private bool _isShooting;

        private float _baseRotation;
        private float _currentRotation;
        private int _currentDirection;
        
        private Animator _animator;
        
        // Unity functions
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _baseRotation = rotationPoint.rotation.eulerAngles.z;
            
            rotateClockwise *= -1;
            _currentDirection = rotateClockwiseFirst ? -1 : 1;

            CheckConsistancy();
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void FixedUpdate() {
            Rotate();
        }

        // Private functions
        private void OnLevelStart() {
            StartCoroutine(ShootPlayer());
        }
        
        private IEnumerator ShootPlayer() {
            while (true) {
                yield return new WaitUntil(() => _isShooting);
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                Vector2 direction = DirectionToPlayer;
                bullet.GetComponent<Bullet>().Shoot(direction, gameObject);
                yield return new WaitForSeconds(shootInterval);
            }
        }

        private void Rotate() {
            if (!rotate) return;

            if (_isShooting) {
                float angleToPlayer = Vector3.SignedAngle(rotationPoint.up, DirectionToPlayer, Vector3.forward);
                if (!(Mathf.Abs(angleToPlayer) < playerLockRotationSpeed)) {
                    _currentDirection = (angleToPlayer < 0) ? -1 : 1;
                    _currentRotation += playerLockRotationSpeed * _currentDirection;
                    _currentRotation = Mathf.Clamp(_currentRotation, rotateClockwise, rotateCounterClockwise);
                    UpdateRotation();
                } else {
                    _currentRotation += angleToPlayer;
                    UpdateRotation();
                }
            } else {
                float rotationThisFrame = rotationSpeed * _currentDirection;
                _currentRotation += rotationThisFrame;

                if (_currentDirection > 0 && _currentRotation > rotateCounterClockwise) {
                    _currentDirection *= -1;
                } else if (_currentDirection < 0 && _currentRotation < rotateClockwise) {
                    _currentDirection *= -1;
                }
                UpdateRotation();
            }
        }
        
        private void CheckConsistancy() {
            if ((int)rotateClockwise == (int)rotateCounterClockwise) {
                Debug.LogWarning("Rotate clockwise and counter clockwise are the same. Turning rotation off");
                rotate = false;
            }
        }

        private void UpdateRotation() {
            rotationPoint.rotation = Quaternion.Euler(new (0,0, (_currentRotation + _baseRotation + 360) % 360));
        }
        
        // Internal functions
        internal void StartShooting() {
            _animator.SetBool(AnimationConstants.SecurityCamera.LockedOnPlayer, true);
            _isShooting = true;
        }
        
        internal void StopShooting() {
            _animator.SetBool(AnimationConstants.SecurityCamera.LockedOnPlayer, false);
            _isShooting = false;
        }
    }
}