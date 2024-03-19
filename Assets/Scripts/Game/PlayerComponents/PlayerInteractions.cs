using Game.GameManagement;
using Game.NightLevels;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerInteractions : MonoBehaviour {
        private bool _interactionsOn = false;
        private PlayerAnimator _playerAnimator;
        
        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }

        private void Awake() {
            _playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_interactionsOn) return;
            switch (other.gameObject.tag) {
                case TagConstants.Oucher:
                    Debug.Log("Hit by: " + other.gameObject.name);
                    GameManager.PlayerDiedStatic();
                    _playerAnimator.PlayDeathAnimation();
                    if (other.TryGetComponent(out Oucher oucher)) {
                        oucher.KilledPlayer();
                    }
                    break;
                case TagConstants.Goal:
                    GameManager.LevelCompletedStatic();
                    break;
            }
        }
        
        // Private functions
        private void OnLevelStart() {
            _interactionsOn = true;
        }
        private void OnLevelOver() {
            _interactionsOn = false;
        }
    }
}