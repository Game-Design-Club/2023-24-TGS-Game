using System;

using Constants;

using Game.PlayerComponents;

using UnityEngine;

namespace Game.NightLevels.FanBlower {
    public class FanTrigger : MonoBehaviour {
        internal event Action<Player> PlayerEntered;
        internal event Action PlayerExited;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                PlayerEntered?.Invoke(other.GetComponent<Player>());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                PlayerExited?.Invoke();
            }
        }
    }
}