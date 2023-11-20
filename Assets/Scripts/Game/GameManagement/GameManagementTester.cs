using System;

using TesterScript;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManagementTester : Tester{
        private void OnEnable() {
            GameManager.Instance.OnLevelOver += LevelOverCalled;
            GameManager.Instance.OnLevelStart += LevelStartCalled;
            GameManager.Instance.OnGamePause += GamePauseCalled;
            GameManager.Instance.OnGameResume += GameResumeCalled;
        }

        private void OnDisable() {
            GameManager.Instance.OnLevelOver -= LevelOverCalled;
            GameManager.Instance.OnLevelStart -= LevelStartCalled;
            GameManager.Instance.OnGamePause -= GamePauseCalled;
            GameManager.Instance.OnGameResume -= GameResumeCalled;
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
        
        [ContextMenu(itemName: "Game Pause")]
        public void GamePause() {
            GameManager.Instance.GamePause();
        }
        
        [ContextMenu(itemName: "Game Resume")]
        public void GameResume() {
            GameManager.Instance.GameResume();
        }
    }
}