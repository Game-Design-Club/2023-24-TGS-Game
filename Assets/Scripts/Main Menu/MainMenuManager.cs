using System;

using AppCore;

using Game.GameManagement.UIManagement;

using Tools.Constants;

using UI;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        [SerializeField] private Animator menuAnimator;
        [SerializeField] private EventSystemManager eventSystemManager;
        [SerializeField] private GameObject defaultButton;
        [SerializeField] private GameObject defaultOptionsButton;
        [SerializeField] private GameObject defaultConfirmButton;
        
        [SerializeField] private Checkbox sfxToggle;
        [SerializeField] private Checkbox musicToggle;
        
        private bool _freeze = false;
        private CurrentMenu _currentMenu = CurrentMenu.Main;
        
        // Unity functions
        private void Start() {
            SetSFXToggle(App.PlayerDataManager.AreSFXOn);
            SetMusicToggle(App.PlayerDataManager.IsMusicOn);
            App.AudioManager.musicPlayer.PlayMainMenuMusic();
        }

        private void OnEnable() {
            App.InputManager.OnCancel += OnCancel;
        }

        private void OnDisable() {
            App.InputManager.OnCancel -= OnCancel;
        }

        // Public functions
        public void StartGame() {
            if (_freeze) return;
            _freeze = true;
            App.SceneManager.LoadScene(SceneConstants.Game);
        }

        public void ShowCredits() { 
            if (_freeze) return;
            
            _freeze = true;
            App.SceneManager.LoadScene(SceneConstants.Credits);
        }

        public void QuitGame() {
            if (_freeze) return;
            _freeze = true;
            
            if (Application.isEditor) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
            Application.Quit();
        }
        
        public void SetSFXToggle(bool value) {
            if (_freeze) return;
            
            App.AudioManager.sfx.SetVolume(value ? 1 : 0);
            App.PlayerDataManager.AreSFXOn = value;
            
            sfxToggle.SetState(value);
        }
        
        public void SetMusicToggle(bool value) {
            if (_freeze) return;
            
            App.PlayerDataManager.IsMusicOn = value;
            App.AudioManager.music.Mute(!value);
            
            musicToggle.SetState(value);
        }
        
        public void ShowOptions() {
            if (_freeze) return;
            
            eventSystemManager.SetSelectedGameObject(defaultOptionsButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.ShowOptions);
            
            _currentMenu = CurrentMenu.Options;
        }
        
        public void HideOptions() {
            if (_freeze) return;
            
            eventSystemManager.SetSelectedGameObject(defaultButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.HideOptions);
            
            _currentMenu = CurrentMenu.Main;
        }
        
        public void ShowConfirm() {
            if (_freeze) return;
            
            eventSystemManager.SetSelectedGameObject(defaultConfirmButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.ShowConfirm);
            
            _currentMenu = CurrentMenu.Confirm;
        }

        public void HideConfirmDeleted() {
            if (_freeze) return;
            
            App.PlayerDataManager.EraseLevelProgress();
            HideConfirm();
        }

        public void HideConfirmBailed() {
            if (_freeze) return;
            
            HideConfirm();
        }
        
        // Private functions
        private void HideConfirm() {
            eventSystemManager.SetSelectedGameObject(defaultOptionsButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.HideConfirm);
            
            _currentMenu = CurrentMenu.Options;
        }

        private void OnCancel() {
            switch (_currentMenu) {
                case CurrentMenu.Main:
                    break;
                case CurrentMenu.Options:
                    HideOptions();
                    break;
                case CurrentMenu.Confirm:
                    HideConfirm();
                    break;
            }
        }
    }
    
    enum CurrentMenu {
        Main,
        Options,
        Confirm
    }
}