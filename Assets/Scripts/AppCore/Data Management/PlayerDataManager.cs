using System;
using System.Collections.Generic;

using UnityEngine;

namespace AppCore.Data_Management {
    public class PlayerDataManager : MonoBehaviour {
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
        
        public int LastCompletedLevelIndex {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.Levels, 0);
            }
            private set {
                PlayerPrefs.SetInt(PlayerDataKeys.Levels, value);
                PlayerPrefs.Save();
            }
        }
        
        public long BestTime {
            get {
                return PlayerPrefs.GetInt(PlayerDataKeys.BestTime, 0);
            }
            private set {
                PlayerPrefs.SetInt(PlayerDataKeys.BestTime, (int)value);
                PlayerPrefs.Save();
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
            LastCompletedLevelIndex++;
        }
        
        public void LogTime(long time) {
            if (time < BestTime) {
                BestTime = time;
            }
        }
        
        public void EraseProgress() {
            PlayerPrefs.DeleteKey(PlayerDataKeys.Levels);
            PlayerPrefs.Save();
        }
    }
}