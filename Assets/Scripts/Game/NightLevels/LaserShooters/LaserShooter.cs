using System.Collections;

using Constants;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.LaserShooters {
    public class LaserShooter : MonoBehaviour {
        [SerializeField] private LaserShooterType laserType;
        [SerializeField] private float betweenShotsTime = 1f;
        [SerializeField] private float warningTime = .5f;
        [SerializeField] private float activeTime = 1f;
        [SerializeField] private float startOffset;
        [SerializeField] private GameObject[] matchLengthLaserParts;
        [SerializeField] private GameObject warningGameObject;
        [SerializeField] private GameObject onGameObject;
        
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

        private void Update() {
            DetermineLaserLength();
        }

        // Private functions
        private void OnLevelStart() {
            if (laserType == LaserShooterType.Cycled) {
                StartCoroutine(WaitToShootLaser());
            } else {
                warningGameObject.SetActive(false);
                onGameObject.SetActive(true);
            }
            DetermineLaserLength();
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private void DetermineLaserLength() {
            Transform thisTransform = transform;
            RaycastHit2D hit = Physics2D.Raycast(thisTransform.position, thisTransform.right, Mathf.Infinity, LayerMask.GetMask(LayerConstants.Walls));
            if (hit.collider is null) {
                Debug.LogError("Laser hit nothing.", this);
                return;
            }
            
            float distance = hit.distance;
            if (distance == _lastDistance) return;
            _lastDistance = distance;
            foreach (GameObject lastComponent in matchLengthLaserParts) {
                Vector3 currentScale = lastComponent.transform.localScale;
                lastComponent.transform.localScale = new Vector3(distance, currentScale.y, currentScale.z);
                Vector3 startPoint = transform.position;
                Vector3 endPoint = hit.point;
                lastComponent.transform.position = (startPoint + endPoint) / 2;
                lastComponent.transform.right = endPoint - startPoint;
            }
        }

        private IEnumerator WaitToShootLaser() {
            warningGameObject.SetActive(false);
            onGameObject.SetActive(false);
            yield return new WaitForSeconds(startOffset);
            StartCoroutine(ShootLaser());
        }
        
        private IEnumerator ShootLaser() {
            while (true) {
                warningGameObject.SetActive(true);
                onGameObject.SetActive(false);
                yield return new WaitForSeconds(warningTime);
                warningGameObject.SetActive(false);
                onGameObject.SetActive(true);
                yield return new WaitForSeconds(activeTime);
                warningGameObject.SetActive(false);
                onGameObject.SetActive(false);
                yield return new WaitForSeconds(betweenShotsTime);
            }
        }
    }
}