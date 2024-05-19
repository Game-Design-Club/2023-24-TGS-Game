using System;

using Game.GameManagement;
using Game.GameManagement.PauseManagement;

using UnityEngine;

namespace AppCore.SpeedrunTimer {
    public class TimerManager : MonoBehaviour {
        [SerializeField] private TimerText timerText;
        
        private float _currentTime = 0;
        private bool _shouldUpdate = false;
        
        public void StartTimer() {
            timerText.Show(App.PlayerDataManager.ShowTimer);
            _currentTime = App.PlayerDataManager.SpeedrunTime;
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += UpdateShowState;
            App.InputManager.OnPlayerStartMovement += OnTimerResume;
            GameManagerEvents.OnLevelOver += OnTimerPause;
            PauseManagerEvents.OnGamePause += OnTimerPause;
            PauseManagerEvents.OnGameResume += OnTimerResume;
        }

        private void OnDisable() {
            GameManagerEvents.OnLevelStart += UpdateShowState;
            App.InputManager.OnPlayerStartMovement -= OnTimerResume;
            GameManagerEvents.OnLevelOver -= OnTimerPause;
            PauseManagerEvents.OnGamePause -= OnTimerPause;
            PauseManagerEvents.OnGameResume -= OnTimerResume;
        }

        private void Update() {
            if (!_shouldUpdate) return;
            _currentTime += Time.deltaTime;
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
    }
}