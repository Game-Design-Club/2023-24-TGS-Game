using System;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace Game.GeneralTrigger {
    public class Trigger : MonoBehaviour {
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private bool onlyPlayer = true;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (onlyPlayer && !other.CompareTag(TagConstants.Player)) return;
            onTriggerEnter?.Invoke();
        }
    }
}