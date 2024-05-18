using AppCore;
using AppCore.DialogueManagement;

using Tools.Constants;

using UnityEngine;

namespace Opening
{
    public class OpeningCutscene : MonoBehaviour {
        [SerializeField] private Dialogue _dialogue;
        private void Start() {
            App.DialogueManager.StartDialogue(_dialogue);
            App.DialogueManager.OnDialogueEnd += OnDialogueEnd;
        }
    
        private void OnDialogueEnd() {
            App.DialogueManager.OnDialogueEnd -= OnDialogueEnd;
            App.SceneManager.LoadScene(SceneConstants.Game);
            App.PlayerDataManager.HasPlayedOpeningDialogue = true;
        }
    }
}
