using UnityEngine;
using UnityEngine.Events;

namespace Game.NightLevels {
    public class Oucher : MonoBehaviour {
        [SerializeField] private UnityEvent onKilledPlayer;
        public void KilledPlayer() {
            onKilledPlayer?.Invoke();
        }
    }
}