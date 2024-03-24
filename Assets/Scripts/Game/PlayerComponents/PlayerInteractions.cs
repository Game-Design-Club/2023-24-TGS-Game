using Game.GameManagement;
using Game.NightLevels;

using Tools.Constants;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerInteractions : MonoBehaviour {
        // Handles interactions between player and other essential game objects
        // Ex. things that kill player, advance level, etc.
        
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