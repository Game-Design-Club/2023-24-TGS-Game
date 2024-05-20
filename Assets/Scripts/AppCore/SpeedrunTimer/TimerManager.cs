using System;

using Game.GameManagement;
using Game.GameManagement.PauseManagement;

using UnityEngine;

namespace AppCore.SpeedrunTimer {
    public class TimerManager : MonoBehaviour {
        [SerializeField] private TimerText timerText;
        
        private float _currentTime = 0;
        private bool _shouldUpdate = false;
        private float _currentLevelTime = 0;
        
        
        public void StartTimer() {
            timerText.Show(App.PlayerDataManager.ShowTimer);
            _currentTime = App.PlayerDataManager.SpeedrunTime;
            UpdateShowState();
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += LevelStart;
            GameManagerEvents.OnLevelOver += LevelEnd;
            App.InputManager.OnPlayerStartMovement += OnTimerResume;
            GameManagerEvents.OnLevelOver += OnTimerPause;
            PauseManagerEvents.OnGamePause += OnTimerPause;
            PauseManagerEvents.OnGameResume += OnTimerResume;
        }

        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= LevelStart;
            GameManagerEvents.OnLevelOver -= LevelEnd;
            App.InputManager.OnPlayerStartMovement -= OnTimerResume;
            GameManagerEvents.OnLevelOver -= OnTimerPause;
            PauseManagerEvents.OnGamePause -= OnTimerPause;
            PauseManagerEvents.OnGameResume -= OnTimerResume;
        }

        private void Update() {
            if (!_shouldUpdate) return;
            _currentTime += Time.deltaTime;
            _currentLevelTime += Time.deltaTime;
            timerText.UpdateText(_currentTime);
        }

        public void OnTimerResume() {
            _shouldUpdate = true;
        }

        public void OnTimerPause() {
            _shouldUpdate = false;
            App.PlayerDataManager.SpeedrunTime = _currentTime;
        }

        public void ResetTimer() {
            App.PlayerDataManager.SpeedrunTime = 0;
            _currentTime = 0;
            timerText.UpdateText(_currentTime);
        }
        
        public void HideTimer() {
            timerText.Show(false);
        }
        
        private void UpdateShowState() {
            timerText.UpdateText(_currentTime);
            timerText.Show(App.PlayerDataManager.ShowTimer);
        }

        private void LevelStart()
        {
            UpdateShowState();
            _currentLevelTime = 0;
        }

        private void LevelEnd()
        {
            // App.SceneManager.
        }
    }
}