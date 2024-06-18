using AppCore;

using Game.GameManagement;
using Game.GameManagement.PauseManagement;
using Game.PlayerComponents;

using UnityEngine;

namespace Tools.Debug_Tools {
    public class ClickToTeleport : MonoBehaviour { // Teleports player to clicked position in world
        [SerializeField] private bool canTeleport = true;
        
        private Player _player;
        
        private void OnEnable() {
            App.InputManager.OnClickWorld += OnClickWorld;
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            App.InputManager.OnClickWorld -= OnClickWorld;
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void OnClickWorld(Vector2 clickPosition) {
            if (!canTeleport) return;
            if (PauseManager.IsPaused) return;
#if UNITY_EDITOR
            _player.transform.position = clickPosition;
#endif
        }
        
        private void OnLevelStart() {
            _player = Player.Instance;
        }
    }
}