using UnityEngine;

namespace Tools.TesterScript {
    public abstract class Tester : MonoBehaviour { // Simple abstract script to test functionality of something
        [SerializeField] private bool debugMessages;
        internal void DebugLog(string message) {
            if (debugMessages) {
                Debug.Log(message);
            }
        }
    }
}