using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using Game.GameManagement.PauseManagement;
using UI;
using UnityEngine;

namespace AppCore.SpeedrunTimer {
    public class TimerManager : MonoBehaviour {
        [SerializeField] private TimerText timerText;
        [SerializeField] private GameObject splitGroup;
        [SerializeField] private GameObject splitPrefab;
        
        private float _currentTime = 0;
        private bool _shouldUpdate = false;
        private List<float> _levelCompleteTimes = new List<float>();
        private SplitText _currentSplitText;
        
        
        public void StartTimer() {
            timerText.Show(App.PlayerDataManager.ShowTimer);
            _currentTime = App.PlayerDataManager.SpeedrunTime;
            UpdateShowState();
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += UpdateShowState;
            GameManagerEvents.OnNextLevel += LevelEnd;
            App.InputManager.OnPlayerStartMovement += OnTimerResume;
            GameManagerEvents.OnLevelOver += OnTimerPause;
            PauseManagerEvents.OnGamePause += OnTimerPause;
            PauseManagerEvents.OnGameResume += OnTimerResume;
            CreateSplit();
        }

        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= UpdateShowState;
            GameManagerEvents.OnLevelOver -= LevelEnd;
            App.InputManager.OnPlayerStartMovement -= OnTimerResume;
            GameManagerEvents.OnLevelOver -= OnTimerPause;
            PauseManagerEvents.OnGamePause -= OnTimerPause;
            PauseManagerEvents.OnGameResume -= OnTimerResume;
        }

        private void Update() {
            if (!_shouldUpdate) return;
            _currentTime += Time.deltaTime;
            timerText.UpdateText(_currentTime);
            _currentSplitText.UpdateSplitText(GetCurrentSplit());
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
            _levelCompleteTimes.Clear();
            timerText.UpdateText(_currentTime);
        }
        
        public void HideTimer() {
            timerText.Show(false);
        }
        
        private void UpdateShowState() {
            timerText.UpdateText(_currentTime);
            _currentSplitText.UpdateSplitText(GetCurrentSplit());
            timerText.Show(App.PlayerDataManager.ShowTimer);
        }

        private void LevelEnd()
        {
            CreateSplit();
        }

        private void CreateSplit()
        {
            _levelCompleteTimes.Add(_currentTime);
            GameObject newSplit = Instantiate(splitPrefab, splitGroup.transform);
            _currentSplitText = newSplit.GetComponent<SplitText>();
            _currentSplitText.UpdateLevelText(_levelCompleteTimes.Count);
        }

        private float GetCurrentSplit()
        {
            return _levelCompleteTimes.Count == 0 ? 0 : _currentTime - _levelCompleteTimes[^1];
        }
    }
}