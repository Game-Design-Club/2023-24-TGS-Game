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
            GameManagerEvents.OnNextLevel += SaveSplit;
            App.InputManager.OnPlayerStartMovement += OnTimerResume;
            GameManagerEvents.OnLevelOver += OnTimerPause;
            PauseManagerEvents.OnGamePause += OnTimerPause;
            PauseManagerEvents.OnGameResume += OnTimerResume;
        }

        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= UpdateShowState;
            GameManagerEvents.OnLevelOver -= SaveSplit;
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
            DestroyChildren();
            _splitTimes = App.PlayerDataManager.SplitTimes;
            
            for (int i = 0 ; i < _splitTimes.Count ; i++)
            {
                float time = _splitTimes[i];
                if (i > 0) time -= _splitTimes[i - 1];
                
                CreateSplit( i + 1, time);
            }

            CreateSplit(_splitTimes.Count + 1);
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
            _splitTimes = new List<float>();
            App.PlayerDataManager.SplitTimes = _splitTimes;
            EnableSplits();
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
            EnableSplits();
            timerText.Show(App.PlayerDataManager.ShowTimer);
            splitGroup.SetActive(App.PlayerDataManager.ShowTimer && App.PlayerDataManager.ShowSplit);
        }
        
        private void SaveSplit()
        {
            _splitTimes.Add(_currentTime);
            App.PlayerDataManager.SplitTimes = _splitTimes;
            
            CreateSplit(_splitTimes.Count);
        }

        private void CreateSplit(int level, float displayTime = 0)
        {
            GameObject newSplit = Instantiate(splitPrefab, splitGroup.transform);
            _currentSplitText = newSplit.GetComponent<SplitText>();
            _currentSplitText.UpdateSplitText(displayTime);
            _currentSplitText.UpdateLevelText(level);
        }

        private float GetCurrentSplit()
        {
            return _splitTimes.Count == 0 ? _currentTime : _currentTime - _splitTimes[^1];
        }


        private void DestroyChildren()
        {
            int children = splitGroup.transform.childCount;
            for (int i = 0 ; i < children; i++){
                Destroy(splitGroup.transform.GetChild(i).gameObject);
            }
        }
    }
}