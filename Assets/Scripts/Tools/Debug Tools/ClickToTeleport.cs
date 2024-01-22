using AppCore;

using Game.GameManagement;
using Game.PlayerComponents;

using UnityEngine;

namespace Tools.Debug_Tools {
    public class ClickToTeleport : MonoBehaviour {
        [SerializeField] private bool canTeleport = true;
        
        private Player _player;
        
        private void OnEnable() {
            App.Instance.inputManager.OnClickWorld += OnClickWorld;
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            App.Instance.inputManager.OnClickWorld -= OnClickWorld;
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void OnClickWorld(Vector2 clickPosition) {
            if (!canTeleport) return;
            _player.transform.position = clickPosition;
        }
        
        private void OnLevelStart() {
            _player = Player.Instance;
        }
    }
}