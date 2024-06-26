using AppCore;
using AppCore.AudioManagement;
using System.Collections;
using System.Collections.Generic;

using Game.GameManagement;
using Game.PlayerComponents;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables {
    [RequireComponent(typeof(Collider2D))]
    public class Interactable : MonoBehaviour { // Base class for all interactable objects
        [SerializeField] private UnityEvent interacted;
        [SerializeField] private bool oneTimeUse = true;
        [SerializeField] private SoundPackage interactSound;
        [SerializeField] private bool playSound = true;
        
        private bool _playerInRange = false;
        private bool _interacted = false;

        private bool _inAnimation = false;
        
        private Collider2D _collider2D;

        private Animator _animator;
        
        // Unity functions
        private void Awake() {
            _collider2D = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
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
        
        private IEnumerator HandleTutorialPopup() {
            if (App.PlayerDataManager.HasInteractedWithButton) yield break;
            
            InteractionsPopup.Show("Press space to interact");
            
            yield return new WaitUntil(() => 
                _interacted);
            
            InteractionsPopup.Hide();
            App.PlayerDataManager.HasInteractedWithButton = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse) || Player.Instance.GetComponent<PlayerBoxMover>().IsGrabbingBox) return;
            
            _playerInRange = true;
            
            _animator.SetBool(AnimationConstants.Interactable.Hover, true);
            StartCoroutine(HandleTutorialPopup());
        }
        
        

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag(TagConstants.Player) || !Application.isPlaying || (_interacted && oneTimeUse) || Player.Instance.GetComponent<PlayerBoxMover>().IsGrabbingBox) return;

            _playerInRange = false;
            
            _animator.SetBool(AnimationConstants.Interactable.Hover, false);
        }
        
        // Private functions
        private void OnInteractPressed() {
            if (!_playerInRange) return;
            Interacted();
        }
        
        private void Interacted() {
            if (_inAnimation) return;
            if (oneTimeUse && _interacted) return;
            interacted?.Invoke();
            if (interactSound != null && playSound) {
                App.AudioManager.PlaySFX(interactSound);
            }

            _animator.SetTrigger(oneTimeUse
                ? AnimationConstants.Interactable.Interact
                : AnimationConstants.Interactable.Click);
            _interacted = true;
        }

        private void PlayingAnimation() {
            _inAnimation = true;
        }
        
        private void AnimationEnded() {
            _inAnimation = false;
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