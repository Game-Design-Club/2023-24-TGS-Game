using Game.PlayerComponents;

using UnityEngine;

namespace Game.NightLevels.FanBlower {
    public class Fan : MonoBehaviour { // Fan that blows the player or other objects
        [SerializeField] private float fanStrength = 5f;
        [SerializeField] private FanTrigger fanTrigger;

        private Player _player;
        private Rigidbody2D _playerRb;
        
        // Unity functions
        private void OnEnable() {
            fanTrigger.PlayerExited += OnPlayerExited;
            fanTrigger.PlayerEntered += OnPlayerEntered;
        }
        
        private void OnDisable() {
            fanTrigger.PlayerExited -= OnPlayerExited;
            fanTrigger.PlayerEntered -= OnPlayerEntered;
        }

        private void Update() {
            // CheckFanLength();
            MovePlayer();
        }
        
        // Private functions
        private void OnPlayerEntered(Player player) {
            _player = player;
            _playerRb = _player.GetComponent<Rigidbody2D>();
        }
        
        private void OnPlayerExited() {
            _player = null;
            _playerRb = null;
        }
        
        private void MovePlayer() {
            if (_player == null) return;
            _playerRb.AddForce(transform.right * fanStrength, ForceMode2D.Force);
            Debug.Log(_playerRb.velocity);
        }
    }
}