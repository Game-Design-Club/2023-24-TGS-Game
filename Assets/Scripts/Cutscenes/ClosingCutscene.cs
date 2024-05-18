using System.Collections;

using AppCore;
using AppCore.DialogueManagement;

using Tools.Constants;

using UnityEngine;

public class ClosingCutscene : MonoBehaviour {
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private float waitTime = 1f;
    
    private void Start() {
        App.DialogueManager.StartDialogue(dialogue);
        App.DialogueManager.OnDialogueEnd += OnDialogueEnd;
    }
    
    private void OnDialogueEnd() {
        App.DialogueManager.OnDialogueEnd -= OnDialogueEnd;
        StartCoroutine(WaitToLeave());
    }
    
    private IEnumerator WaitToLeave() {
        yield return new WaitForSeconds(waitTime);
        App.SceneManager.LoadScene(SceneConstants.Credits);
    }
}
