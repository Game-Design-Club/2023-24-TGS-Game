using Constants;

using Game.GameManagement;
using Game.NightLevels;

using UnityEngine;

namespace Game.PlayerComponents {
    public class PlayerInteractions : MonoBehaviour {
        private bool _interactionsOn = false;
        
        // Unity functions
        private void OnEnable() {
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            GameManager.Instance.OnLevelOver -= OnLevelOver;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!_interactionsOn) return;
            switch (other.gameObject.tag) {
                case TagConstants.Oucher:
                    GameManager.Instance.PlayerDied();
                    if (other.TryGetComponent(out Oucher oucher)) {
                        oucher.KilledPlayer();
                    }
                    break;
                case TagConstants.Goal:
                    GameManager.Instance.LevelCompleted();
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