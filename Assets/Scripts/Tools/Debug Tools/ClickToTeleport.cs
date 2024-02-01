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
            if (!ClickIsInWorld(clickPosition)) return;
            _player.transform.position = clickPosition;
        }
        
        private void OnLevelStart() {
            _player = Player.Instance;
        }
        
        private bool ClickIsInWorld(Vector2 clickPosition) {
            Camera mainCamera = Camera.main;

            Vector2 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
            Vector2 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
            
            return clickPosition.x >= bottomLeft.x && clickPosition.x <= topRight.x &&
                   clickPosition.y >= bottomLeft.y && clickPosition.y <= topRight.y;
        }

    }
}