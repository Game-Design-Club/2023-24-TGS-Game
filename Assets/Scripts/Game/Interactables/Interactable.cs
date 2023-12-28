using AppCore;

using Constants;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour {
        [SerializeField] private UnityEvent interacted;
        
        private bool _playerInRange = false;
        
        private Collider2D _collider2D;
        
        // Unity functions
        private void Awake() {
            _collider2D = GetComponent<Collider2D>();
            if (_collider2D.isTrigger == false) {
                Debug.LogError("Collider2D on Interactable must be a trigger");
            }
        }

        private void OnEnable() {
            App.Instance.inputManager.OnInteractPressed += OnInteractPressed;
            GameManager.Instance.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            App.Instance.inputManager.OnInteractPressed -= OnInteractPressed;
            GameManager.Instance.OnLevelStart -= OnLevelStart;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying) return;

            _playerInRange = true;
            InteractionsPopup.Instance.Show();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying) return;

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
        }
        
        private void OnLevelStart() {
            CheckInitialPlayerPosition();
        }
        
        private void CheckInitialPlayerPosition() {
            // Adjust the size and position based on your trigger collider
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size, 0);
            foreach (Collider2D hitCollider in hitColliders) {
                if (hitCollider.CompareTag(TagConstants.Player)) {
                    _playerInRange = true;
                    break;
                }
            }
        }
    }
}