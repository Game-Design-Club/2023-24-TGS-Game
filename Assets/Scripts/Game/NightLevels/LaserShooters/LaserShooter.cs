using System.Collections;

using Constants;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.LaserShooters {
    public class LaserShooter : MonoBehaviour{
        [SerializeField] private float betweenShotsTime = 1f;
        [SerializeField] private float warningTime = .5f;
        [SerializeField] private float activeTime = 1f;

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
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private IEnumerator ShootLaser() {
            while (true) {
                _animator.SetTrigger(AnimationConstants.LaserShooter.Off);
                Debug.Log("Laser off");
                yield return new WaitForSeconds(betweenShotsTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.Warning);
                Debug.Log("Laser warning");
                yield return new WaitForSeconds(warningTime);
                _animator.SetTrigger(AnimationConstants.LaserShooter.On);
                Debug.Log("Laser on");
                yield return new WaitForSeconds(activeTime);
            }
        }
    }
}