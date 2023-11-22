using UnityEngine;

namespace TesterScript {
    public abstract class Tester : MonoBehaviour{
        [SerializeField] private bool debugMessages;
        internal void DebugLog(string message) {
            if (debugMessages) {
                Debug.Log(message);
            }
        }
    }
}