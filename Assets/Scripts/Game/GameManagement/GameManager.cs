using System;

using AppCore;

using UnityEngine;

namespace Game.GameManagement {
    public class GameManager : MonoBehaviour{
        public static GameManager Instance { get; private set; }
        
        public event Action OnGameStart;
        public event Action OnGameEnd;
        public event Action OnGamePause;
        public event Action OnGameResume;

        public bool IsPaused { get; private set; } // isPaused should only be true if isPlaying is true
        
        private void Awake() {
            if (Instance is null) {
                Instance = this;
                Debug.Log("GameManager initialized.");
            } else {
                Debug.LogWarning("Duplicate GameManager found and deleted.");
                Destroy(gameObject);
            }
        }

        public void GameStart() {
            OnGameStart?.Invoke();
            IsPaused = false;
        }
        
        public void GameEnd() {
            OnGameEnd?.Invoke();
            IsPaused = true;
        }
        
        public void GamePause() {
            if (IsPaused) {
                Debug.LogWarning("Tried to pause game while already paused.");
                return;
            }
            OnGamePause?.Invoke();
            IsPaused = true;
            App.Instance.timeManager.SetTimeScale(0, true);
        }
        
        public void GameResume() {
            if (!IsPaused) {
                Debug.LogWarning("Tried to resume game while not paused.");
                return;
            }
            OnGameResume?.Invoke();
            IsPaused = false;
            App.Instance.timeManager.SetTimeScale(1, true);
        }
    }
}