using AppCore;

using TesterScript;

using UnityEngine;

namespace Game.LevelManagement {
    public class LevelManagementTester : Tester{
        [SerializeField] private LevelManager levelManager;
        
        [ContextMenu(itemName: "Load First Level")]
        public void LoadFirstLevel() {
            levelManager.LoadFirstLevel();
            DebugLog("Loading First Level");
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