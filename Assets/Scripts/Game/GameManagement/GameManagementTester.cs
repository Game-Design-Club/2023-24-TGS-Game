using System;

using Game.GameManagement.LevelManagement;

using TesterScript;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManagementTester : Tester{
        [SerializeField] private LevelManager levelManager;
        private void OnEnable() {
            GameManager.Instance.OnLevelOver += LevelOverCalled;
            GameManager.Instance.OnLevelStart += LevelStartCalled;
            
            levelManager.OnLevelLoaded += LevelLoadedCalled;
        }

        private void OnDisable() {
            GameManager.Instance.OnLevelOver -= LevelOverCalled;
            GameManager.Instance.OnLevelStart -= LevelStartCalled;
            
            levelManager.OnLevelLoaded -= LevelLoadedCalled;
        }

        private void LevelOverCalled() {
            DebugLog("Game End Called");
        }
        
        private void LevelStartCalled() {
            DebugLog("Game Start Called");
        }
        
        private void GamePauseCalled() {
            DebugLog("Game Pause Called");
        }
        
        private void GameResumeCalled() {
            DebugLog("Game Resume Called");
        }
        
        private void LevelLoadedCalled() {
            DebugLog("Level Loaded Called");
        }
        
        [ContextMenu(itemName: "Game Start")]
        public void GameStart() {
            GameManager.Instance.GameStart();
        }
        
        [ContextMenu(itemName: "Player Died")]
        public void GameEnd() {
            GameManager.Instance.PlayerDied();
        }
        
        [ContextMenu(itemName: "Level Finished")]
        public void PlayerWon() {
            GameManager.Instance.LevelFinished();
        }
    }
}