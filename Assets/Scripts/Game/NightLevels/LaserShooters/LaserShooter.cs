using System;
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
        [SerializeField] private float startOffset;
        [SerializeField] private GameObject[] matchLengthLaserParts;

        private Animator _animator;

        private float _lastDistance = 0;
        
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

        private void Update() {
            DetermineLaserLength();
        }

        // Private functions
        private void OnLevelStart() {
            StartCoroutine(WaitToShootLaser());
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
            if (distance == _lastDistance) return;
            Debug.Log($"New Distance Calculated: {distance}");
            _lastDistance = distance;
            foreach (GameObject lastComponent in matchLengthLaserParts) {
                Vector3 currentScale = lastComponent.transform.localScale;
                lastComponent.transform.localScale = new Vector3(distance, currentScale.y, currentScale.z);
                // Set line location to be starting from current position to the hit point, no matter which direction the laser is facing
                Vector3 startPoint = lastComponent.transform.position;
                Vector3 endPoint = hit.point;
                lastComponent.transform.position = (startPoint + endPoint) / 2;
                lastComponent.transform.right = endPoint - startPoint;
                Debug.Log($"Start Point: {startPoint}, End Point: {endPoint}");
            }
        }

        private IEnumerator WaitToShootLaser() {
            yield return new WaitForSeconds(startOffset);
            StartCoroutine(ShootLaser());
        }
        
        private IEnumerator ShootLaser() {
            while (true) {
                _animator.SetTrigger(AnimationConstants.LaserShooter.Warning);
                yield return new WaitForSeconds(warningTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.On);
                yield return new WaitForSeconds(activeTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.Off);
                yield return new WaitForSeconds(betweenShotsTime);
            }
        }
    }
}