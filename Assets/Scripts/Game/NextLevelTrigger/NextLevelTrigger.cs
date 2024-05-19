using System;

using Game.GameManagement;

using UnityEngine;

namespace Game.NextLevelTrigger {
    public class NextLevelTrigger : MonoBehaviour {
        bool _triggered = false;
        public void NextLevel() {
            if (_triggered) return;
            _triggered = true;
            GameManager.LevelCompleted();
        }
    }
}