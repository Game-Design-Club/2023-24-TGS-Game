using System;

using Game.GameManagement;

using UnityEngine;

namespace Game.NextLevelTrigger {
    public class NextLevelTrigger : MonoBehaviour {
        private void Awake() {
            
        }

        public void NextLevel() {
            GameManager.LevelCompleted();
        }
    }
}