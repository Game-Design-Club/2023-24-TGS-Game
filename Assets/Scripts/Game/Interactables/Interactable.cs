using AppCore;

using Game.GameManagement;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour {
        [SerializeField] private UnityEvent interacted;
        [SerializeField] private bool oneTimeUse = true;
        
        private bool _playerInRange = false;
        private bool _interacted = false;
        
        private Collider2D _collider2D;
        
        // Unity functions
        private void Awake() {
            _collider2D = GetComponent<Collider2D>();
            if (_collider2D.isTrigger == false) {
                Debug.LogError("Collider2D on Interactable must be a trigger");
            }
        }

        private void OnEnable() {
            App.Instance.inputManager.OnInteract += OnInteractPressed;
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            App.Instance.inputManager.OnInteract -= OnInteractPressed;
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse)) return;

            _playerInRange = true;
            InteractionsPopup.Instance.Show();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse)) return;

            _playerInRange = false;
            InteractionsPopup.Instance.Hide();
        }
        
        // Private functions
        private void OnInteractPressed() {
            if (!_playerInRange) return;
            Interacted();
        }
        
        private void Interacted() {
            interacted?.Invoke();
            if (oneTimeUse) {
                _interacted = true;
                InteractionsPopup.Instance.Hide();
            }
        }
        
        private void OnLevelStart() {
            CheckInitialPlayerPosition();
        }
        
        private void CheckInitialPlayerPosition() {
            // Adjust the size and position based on your trigger collider
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, _collider2D.bounds.size, 0);
            foreach (Collider2D hitCollider in hitColliders) {
                if (hitCollider.CompareTag(TagConstants.Player)) {
                    _playerInRange = true;
                    break;
                }
            }
        }
    }
}