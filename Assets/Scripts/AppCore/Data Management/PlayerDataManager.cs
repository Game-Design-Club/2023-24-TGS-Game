using UnityEngine;

namespace AppCore.Data_Management {
    public class PlayerDataManager : MonoBehaviour { // Manages player data such as sound settings and level progress
        // All player data uses PlayerPrefs, a built-in Unity class that stores data on the user's device, also works for Unity editor
        // variables are all C# properties, which are like variables but with getter and setter functions, to make sure the data is always saved to PlayerPrefs
        public bool AreSFXOn {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.SFX, 1) == 1;
            }
            private set {
                PlayerPrefs.SetInt(PlayerDataKeys.SFX, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        public bool IsMusicOn {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.Music, 1) == 1;
            }
            private set {
                PlayerPrefs.SetInt(PlayerDataKeys.Music, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        // The last level completed by the player
        public int LastCompletedLevelIndex {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.Levels, 0);
            }
            private set {
                PlayerPrefs.SetInt(PlayerDataKeys.Levels, value);
                PlayerPrefs.Save();
                Debug.Log("int set to " + value);
            }
        }
        
        // Public functions
        public void SetSFX(bool value) {
            AreSFXOn = value;
        }
        
        public void SetMusic(bool value) {
            IsMusicOn = value;
        }
        
        public void LastLevelCompleted(int index) {
            LastCompletedLevelIndex = index;
        }
        
        public void EraseLevelProgress() { // Only erases the level progress
            PlayerPrefs.DeleteKey(PlayerDataKeys.Levels);
            PlayerPrefs.Save();
        }
        
        public void EraseAllData() { // Erases all player data
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}