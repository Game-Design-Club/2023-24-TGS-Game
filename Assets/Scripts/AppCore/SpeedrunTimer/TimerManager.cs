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
        private List<float> _splitTimes;
        private SplitText _currentSplitText;
        
        
        public void StartTimer() {
            timerText.Show(App.PlayerDataManager.ShowTimer);
            splitGroup.SetActive(App.PlayerDataManager.ShowTimer && App.PlayerDataManager.ShowSplit);
            _currentTime = App.PlayerDataManager.SpeedrunTime;
            UpdateShowState();
        }

        private void Start()
        {
            EnableSplits();
        }

        private void OnEnable() {
            GameManagerEvents.OnLevelStart += UpdateShowState;
            GameManagerEvents.OnNextLevel += LevelEnd;
            App.InputManager.OnPlayerStartMovement += OnTimerResume;
            GameManagerEvents.OnLevelOver += OnTimerPause;
            PauseManagerEvents.OnGamePause += OnTimerPause;
            PauseManagerEvents.OnGameResume += OnTimerResume;
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

        private void EnableSplits()
        {
            _splitTimes = App.PlayerDataManager.SplitTimes;
            int lastLevel = 0;
            foreach (float time in _splitTimes)
            {
                CreateSplit(time);
                lastLevel++;
            }

            if (lastLevel == App.PlayerDataManager.LastCompletedLevelIndex)
            {
                CreateSplit();
            }
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
            _splitTimes.Clear();
            timerText.UpdateText(_currentTime);
            _currentSplitText.UpdateSplitText(GetCurrentSplit());
        }
        
        public void HideTimer() {
            timerText.Show(false);
            splitGroup.SetActive(false);
        }
        
        private void UpdateShowState() {
            timerText.UpdateText(_currentTime);
            _currentSplitText.UpdateSplitText(GetCurrentSplit());
            timerText.Show(App.PlayerDataManager.ShowTimer);
            splitGroup.SetActive(App.PlayerDataManager.ShowTimer && App.PlayerDataManager.ShowSplit);
        }

        private void LevelEnd()
        {
            CreateSplit();
            App.PlayerDataManager.SplitTimes = _splitTimes;
        }

        private void CreateSplit(float time = float.NaN)
        {
            _splitTimes.Add(time.Equals(float.NaN) ? _currentTime : time);
            GameObject newSplit = Instantiate(splitPrefab, splitGroup.transform);
            _currentSplitText = newSplit.GetComponent<SplitText>();
            _currentSplitText.UpdateLevelText(_splitTimes.Count);
        }

        private float GetCurrentSplit()
        {
            return _splitTimes.Count == 0 ? _currentTime : _currentTime - _splitTimes[^1];
        }
    }
}