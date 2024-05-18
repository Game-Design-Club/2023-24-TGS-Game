using System.Collections;

using AppCore;
using AppCore.DialogueManagement;

using Tools.Constants;

using UnityEngine;

namespace Cutscenes
{
    public class OpeningCutscene : MonoBehaviour {
        [SerializeField] private Dialogue _dialogue;
        [SerializeField] private float waitTime = 1f;
        private void Start() {
            App.DialogueManager.StartDialogue(_dialogue);
            App.DialogueManager.OnDialogueEnd += OnDialogueEnd;
        }
    
        private void OnDialogueEnd() {
            App.DialogueManager.OnDialogueEnd -= OnDialogueEnd;
            StartCoroutine(WaitToLeave());
        }
        
        private IEnumerator WaitToLeave() {
            yield return new WaitForSeconds(waitTime);
            App.SceneManager.LoadScene(SceneConstants.Game);
        }
    }
}
