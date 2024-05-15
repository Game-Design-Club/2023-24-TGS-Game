using System.Collections;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NightLevels.LaserShooters {
    public class LaserShooter : MonoBehaviour { // Shoots a laser that goes on and off periodically
        [SerializeField] private LaserShooterType laserType;
        [SerializeField] private float betweenShotsTime = 1f;
        [SerializeField] private float warningTime = .5f;
        [SerializeField] private float activeTime = 1f;
        [FormerlySerializedAs("startOffset")] [SerializeField] private float startDelay;
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
            switch (laserType) {
                case LaserShooterType.Cycled:
                    StartCoroutine(ShootLaser());
                    break;
                case LaserShooterType.Static:
                    warningGameObject.SetActive(false);
                    onGameObject.SetActive(true);
                    break;
            }
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private IEnumerator ShootLaser() {
            warningGameObject.SetActive(false);
            onGameObject.SetActive(false);
            yield return new WaitForSeconds(startDelay);
            
            // Main loop
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
        
        // Public functions
        public void SetActive(bool active) {
            switch (laserType) {
                case LaserShooterType.Cycled:
                    if (active) {
                        StartCoroutine(ShootLaser());
                    } else {
                        StopAllCoroutines();
                    }
                    break;
                case LaserShooterType.Static:
                    warningGameObject.SetActive(active);
                    onGameObject.SetActive(active);
                    break;
            }
        }
    }
}