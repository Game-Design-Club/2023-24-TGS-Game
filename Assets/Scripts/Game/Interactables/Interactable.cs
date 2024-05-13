using AppCore;
using AppCore.AudioManagement;

using Game.GameManagement;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour { // Base class for all interactable objects
        [SerializeField] private UnityEvent interacted;
        [SerializeField] private bool oneTimeUse = true;
        [SerializeField] private bool destroyOnInteract = true;
        [SerializeField] private SoundPackage interactSound;
        
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
            App.InputManager.OnInteract += OnInteractPressed;
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            App.InputManager.OnInteract -= OnInteractPressed;
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse)) return;

            _playerInRange = true;
            InteractionsPopup.Show();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse)) return;

            _playerInRange = false;
            InteractionsPopup.Hide();
        }
        
        // Private functions
        private void OnInteractPressed() {
            if (!_playerInRange) return;
            Interacted();
        }
        
        private void Interacted() {
            interacted?.Invoke();
            if (interactSound != null) {
                App.AudioManager.PlaySFX(interactSound);
            }
            if (oneTimeUse) {
                _interacted = true;
                InteractionsPopup.Hide();
                if (destroyOnInteract) {
                    gameObject.SetActive(false);
                }
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