using UnityEngine;

namespace TesterScript {
    public class Tester : MonoBehaviour{
        [SerializeField] private bool debugMessages;
        internal void DebugLog(string message) {
            if (debugMessages) {
                Debug.Log(message);
            }
        }
    }
}