using System.Collections;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace Game.GeneralTrigger {
    public class GeneralOnStart : MonoBehaviour {
        [SerializeField] private UnityEvent onStart;
        
        private void Start() {
            StartCoroutine(Trigger());
        }

        private IEnumerator Trigger() {
            yield return null;
            onStart?.Invoke();
        }
    }
}