using System.Collections;

using Constants;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NightLevels.LaserShooters {
    public class LaserShooter : MonoBehaviour{
        [SerializeField] private float betweenShotsTime = 1f;
        [SerializeField] private float warningTime = .5f;
        [SerializeField] private float activeTime = 1f;

        [SerializeField] private GameObject[] matchLengthLaserParts;

        private Animator _animator;
        
        // Unity functions
        private void OnEnable() {
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            GameManager.Instance.OnLevelOver -= OnLevelOver;
        }

        private void Awake() {
            _animator = GetComponent<Animator>();
            if (_animator is null) {
                Debug.LogError("Animator not found.", this);
            }
        }

        // Private functions
        private void OnLevelStart() {
            StartCoroutine(ShootLaser());
            DetermineLaserLength();
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private void DetermineLaserLength() {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, LayerMask.GetMask(LayerConstants.Walls));
            if (hit.collider is null) {
                Debug.LogError("Laser hit nothing.", this);
                return;
            }

            float distance = hit.distance;
            foreach (GameObject laser in matchLengthLaserParts) {
                Vector3 currentScale = laser.transform.localScale;
                laser.transform.localScale = new Vector3(distance, currentScale.y, currentScale.z);
                // Set line location to be starting from current position to the hit point, no matter which direction the laser is facing
                Vector3 startPoint = laser.transform.position;
                Vector3 endPoint = hit.point;
                laser.transform.position = (startPoint + endPoint) / 2;
                laser.transform.right = endPoint - startPoint;
                
            }
        }
        
        private IEnumerator ShootLaser() {
            while (true) {
                yield return new WaitForSeconds(betweenShotsTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.Warning);
                yield return new WaitForSeconds(warningTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.On);
                yield return new WaitForSeconds(activeTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.Off);
            }
        }
    }
}