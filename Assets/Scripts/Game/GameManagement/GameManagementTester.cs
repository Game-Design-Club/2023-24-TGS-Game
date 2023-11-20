using System;

using TesterScript;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManagementTester : Tester{
        private void OnEnable() {
            GameManager.Instance.OnGameEnd += GameEndCalled;
            GameManager.Instance.OnGameStart += GameStartCalled;
            GameManager.Instance.OnGamePause += GamePauseCalled;
            GameManager.Instance.OnGameResume += GameResumeCalled;
        }

        private void OnDisable() {
            GameManager.Instance.OnGameEnd -= GameEndCalled;
            GameManager.Instance.OnGameStart -= GameStartCalled;
            GameManager.Instance.OnGamePause -= GamePauseCalled;
            GameManager.Instance.OnGameResume -= GameResumeCalled;
        }

        private void GameEndCalled() {
            DebugLog("Game End Called");
        }
        
        private void GameStartCalled() {
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
        
        [ContextMenu(itemName: "Game End")]
        public void GameEnd() {
            GameManager.Instance.GameEnd();
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