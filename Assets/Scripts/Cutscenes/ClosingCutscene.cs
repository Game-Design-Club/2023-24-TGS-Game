using System;
using System.Collections;

using AppCore;
using AppCore.DialogueManagement;

using Tools.Constants;

using UnityEngine;

public class ClosingCutscene : MonoBehaviour {
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private float waitTime = 1f;

    private Animator _animator;
    
    private void Awake() {
        _animator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(waitTime);
        App.SceneManager.LoadScene(SceneConstants.Credits);
    }
}
