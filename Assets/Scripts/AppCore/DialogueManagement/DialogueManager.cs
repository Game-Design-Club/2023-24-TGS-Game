using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace AppCore.DialogueManagement {
    public class DialogueManager : MonoBehaviour {
        [SerializeField] private Dialogue testDialogue;
        
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private Image characterSpriteRenderer;
        
        private Dialogue _currentDialogue;
        
        private bool _shouldContinue;
        
        // Unity functions
        private void Start() {
            dialogueBox.SetActive(false);
            if (testDialogue != null) {
                StartDialogue(testDialogue);
            }
        }

        private void OnEnable() {
            App.InputManager.OnDialogueContinue += OnContinue;
        }
        
        private void OnDisable() {
            App.InputManager.OnDialogueContinue -= OnContinue;
        }

        // Private functions
        private IEnumerator PlayDialogue() {
            App.InputManager.LockedControlsList.Add(this);
            App.InputManager.LockedUIList.Add(this);
            dialogueBox.SetActive(true);
            foreach (DialogueChunk currentChunk in _currentDialogue) {
                PlayDialogueChunk(currentChunk);
                yield return new WaitUntil(() => _shouldContinue);
                _shouldContinue = false;
            }
            _currentDialogue = null;
            dialogueBox.SetActive(false);
            App.InputManager.LockedControlsList.Remove(this);
            App.InputManager.LockedUIList.Remove(this);
        }

        private void OnContinue() {
            _shouldContinue = true;
        }
        
        private void PlayDialogueChunk(DialogueChunk chunk) {
            characterNameText.text = chunk.character.characterName;
            dialogueText.text = chunk.text;
            dialogueText.color = chunk.character.textColor;
            characterSpriteRenderer.sprite = chunk.character.characterSprite;
        }
        
        // Public functions
        public void StartDialogue(Dialogue dialogue) {
            if (_currentDialogue != null) {
                Debug.LogWarning(_currentDialogue == dialogue
                    ? "Tried to play cutscene while already playing that dialogue"
                    : "Tried to play a cutscene while playing another dialogue");
            }
            
            _currentDialogue = dialogue;
            StartCoroutine(PlayDialogue());
        }
        
    }
}