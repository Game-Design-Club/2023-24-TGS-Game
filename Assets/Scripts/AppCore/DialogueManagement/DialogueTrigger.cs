using Tools.Constants;

using UnityEngine;

namespace AppCore.DialogueManagement {
    public class DialogueTrigger : MonoBehaviour { // Allows a trigger collider or another class to begin dialogue
        [SerializeField] public Dialogue dialogue;
        [SerializeField] private bool triggerOnce = true;
        
        private bool _hasTriggered;
        
        // Unity functions
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                TriggerDialogue();
            }
        }
        
        // Public functions
        public void TriggerDialogue() {
            if (triggerOnce && _hasTriggered) return;
            App.DialogueManager.StartDialogue(dialogue);
            _hasTriggered = true;
        }
    }
}