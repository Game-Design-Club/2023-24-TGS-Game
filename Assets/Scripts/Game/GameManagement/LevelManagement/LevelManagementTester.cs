using Tools.TesterScript;

using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    public class LevelManagementTester : Tester { // Tests the LevelManager
        [SerializeField] private LevelManager levelManager;
        
        [ContextMenu(itemName: "Load First Level")]
        public void LoadFirstLevel() {
            levelManager.LoadFirstLevel();
            DebugLog("Loading First Level");
        }
        
        [ContextMenu(itemName: "Load Saved Level")]
        public void LoadSavedLevel() {
            levelManager.LoadSavedLevel();
            DebugLog("Loading Saved Level");
        }
        
        [ContextMenu(itemName: "Load Next Level")]
        public void LoadNextLevel() {
            levelManager.LoadNextLevel();
            DebugLog("Loading Next Level");
        }
        
        [ContextMenu(itemName: "Restart Level")]
        public void RestartLevel() {
            levelManager.RestartLevel();
            DebugLog("Restarting Level");
        }
    }
}