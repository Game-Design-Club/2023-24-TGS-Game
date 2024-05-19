using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace AppCore.DialogueManagement {
    public class DialogueTrigger : MonoBehaviour { // Allows a trigger collider or another class to begin dialogue
        [SerializeField] public Dialogue dialogue;
        [SerializeField] private UnityEvent onDialogueFinish;
        
        private bool _hasTriggered;
        
        // Unity functions
        private void Awake() {
            if (dialogue == null) {
                Debug.LogWarning("Dialogue not set in DialogueTrigger.");
            }

            if (dialogue.IsEmpty) {
                Debug.LogWarning("Dialogue is empty in DialogueTrigger.");
            }
            
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger == null) {
                Debug.LogWarning("No Collider2D found on DialogueTrigger.");
            } else if (!trigger.isTrigger) {
                Debug.LogWarning("Collider2D on DialogueTrigger is not set to trigger.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(TagConstants.Player)) {
                TriggerDialogue();
            }
        }
        
        // Public functions
        public void TriggerDialogue() {
            if (_hasTriggered) return;
            _hasTriggered = true;
            if (App.PlayerDataManager.HasTriggeredDialogue(dialogue)) {
                onDialogueFinish.Invoke();
                return;
            }
            App.DialogueManager.StartDialogue(dialogue);
            App.DialogueManager.OnDialogueEnd += OnDialogueEnd;
        }
        
        // Private functions
        private void OnDialogueEnd() {
            App.DialogueManager.OnDialogueEnd -= OnDialogueEnd;
            onDialogueFinish.Invoke();
        }
    }
}