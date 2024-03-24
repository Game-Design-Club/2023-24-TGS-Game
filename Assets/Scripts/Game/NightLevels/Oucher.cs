using UnityEngine;
using UnityEngine.Events;

namespace Game.NightLevels {
    public class Oucher : MonoBehaviour { // Default ouch event, if the thing that killed the player needs to know when that happened
        [SerializeField] private UnityEvent onKilledPlayer;
        public void KilledPlayer() {
            onKilledPlayer?.Invoke();
        }
    }
}