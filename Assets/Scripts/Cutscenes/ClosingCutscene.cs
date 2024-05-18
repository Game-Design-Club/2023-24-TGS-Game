using System.Collections;

using AppCore;
using AppCore.DialogueManagement;

using Tools.Constants;

using UnityEngine;

namespace Cutscenes
{
    public class ClosingCutscene : MonoBehaviour {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private float waitTime = 1f;

        private Animator _animator;
    
        private void Awake() {
            _animator = GetComponentInParent<Animator>();
        }

        private void Start() {
            App.DialogueManager.StartDialogue(dialogue);
            App.DialogueManager.OnDialogueEnd += OnDialogueEnd;
        }
    
        private void OnDialogueEnd() {
            App.DialogueManager.OnDialogueEnd -= OnDialogueEnd;
            StartCoroutine(WaitToLeave());
        }
    
        private IEnumerator WaitToLeave() {
            _animator.SetTrigger(AnimationConstants.ClosingCutscene.Play);
            yield return new WaitForSecondsRealtime(waitTime);
            App.SceneManager.LoadScene(SceneConstants.Credits);
        }
    }
}
