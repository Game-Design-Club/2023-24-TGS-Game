using System;
using System.Collections.Generic;

using AppCore.DialogueManagement;

using UnityEngine;

namespace AppCore.Data_Management {
    public class PlayerDataManager : MonoBehaviour { // Manages player data such as sound settings and level progress
        // All player data uses PlayerPrefs, a built-in Unity class that stores data on the user's device, also works for Unity editor
        // variables are all C# properties, which are like variables but with getter and setter functions, to make sure the data is always saved to PlayerPrefs
        public float MasterLevel {
            get {
                return PlayerPrefs.GetFloat(PlayerDataKeys.Master, 1);
            }
            set {
                PlayerPrefs.SetFloat(PlayerDataKeys.Master, value);
                PlayerPrefs.Save();
            }
        }
        
        public float MusicLevel {
            get {
                return PlayerPrefs.GetFloat(PlayerDataKeys.Music, 1);
            }
            set {
                PlayerPrefs.SetFloat(PlayerDataKeys.Music, value);
                PlayerPrefs.Save();
            }
        }
        
        public float SFXLevel {
            get {
                return PlayerPrefs.GetFloat(PlayerDataKeys.SFX, 1);
            }
            set {
                PlayerPrefs.SetFloat(PlayerDataKeys.SFX, value);
                PlayerPrefs.Save();
            }
        }
        
        // The last level completed by the player
        public int LastCompletedLevelIndex {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.Levels, 0);
            }
            set {
                PlayerPrefs.SetInt(PlayerDataKeys.Levels, value);
                PlayerPrefs.Save();
            }
        }

        public bool HasInteracted {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.TutorialHasInteracted, 0) == 1;
            }
            set {
                PlayerPrefs.SetInt(PlayerDataKeys.TutorialHasInteracted, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        private HashSet<String> _dialogueKeys = new HashSet<String>();
        
        // Unity functions
        private void Awake() {
            _dialogueKeys = GetDialogueKeys();
        }
        
        // Private functions
        private HashSet<String> GetDialogueKeys() {
            String[] keys = PlayerPrefs.GetString(PlayerDataKeys.DialogueKeys, "").Split(',');
            HashSet<String> dialogueKeys = new HashSet<String>();
            foreach (String key in keys) {
                if (!String.IsNullOrEmpty(key)) {
                    dialogueKeys.Add(key);
                }
            }
            return dialogueKeys;
        }

        // Public functions
        public void DialogueCompleted(Dialogue dialogue) {
            PlayerPrefs.SetInt(PlayerDataKeys.Dialogue + dialogue.dialogueKey, 1);
            PlayerPrefs.Save();
            _dialogueKeys.Add(dialogue.dialogueKey);
            PlayerPrefs.SetString(PlayerDataKeys.DialogueKeys, String.Join(",", _dialogueKeys));
        }
        
        public bool HasTriggeredDialogue(Dialogue dialogue) {
            return PlayerPrefs.GetInt(PlayerDataKeys.Dialogue + dialogue.dialogueKey, 0) == 1;
        }
        
        public void LastLevelCompleted(int index) {
            LastCompletedLevelIndex = index;
        }
        
        public void EraseLevelProgress() { // Only erases the level progress
            PlayerPrefs.DeleteKey(PlayerDataKeys.Levels);
            PlayerPrefs.DeleteKey(PlayerDataKeys.TutorialHasInteracted);
            foreach (String key in _dialogueKeys) {
                PlayerPrefs.DeleteKey(PlayerDataKeys.Dialogue + key);
            }
            PlayerPrefs.Save();
        }
        
        [ContextMenu("Erase All Data")]
        public void EraseAllData() { // Erases all player data
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}