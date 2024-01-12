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
        [SerializeField] private GameObject warningGameObject;
        [SerializeField] private GameObject onGameObject;
        
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
        private void OnLevelStart() {
            if (laserType == LaserShooterType.Cycled) {
                StartCoroutine(WaitToShootLaser());
            } else {
                warningGameObject.SetActive(false);
                onGameObject.SetActive(true);
            }
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
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