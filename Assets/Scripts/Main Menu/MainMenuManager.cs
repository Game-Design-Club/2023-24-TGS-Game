using AppCore;
using AppCore.AudioManagement;

using Game.GameManagement.UIManagement;

using Tools.Constants;

using UI;

using UnityEngine;
using UnityEngine.UI;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        [SerializeField] private Animator menuAnimator;
        [SerializeField] private EventSystemManager eventSystemManager;
        [SerializeField] private GameObject defaultButton;
        [SerializeField] private GameObject defaultOptionsButton;
        [SerializeField] private GameObject defaultConfirmButton;
        
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Checkbox timerCheckbox;
        
        [SerializeField] private Music menuMusic;
        
        private bool _freeze = false;
        private CurrentMenu _currentMenu = CurrentMenu.Main;
        
        // Unity functions
        private void Start() {
            SetMasterLevel(App.PlayerDataManager.MasterLevel);
            SetMusicLevel(App.PlayerDataManager.MusicLevel);
            SetSFXLevel(App.PlayerDataManager.SFXLevel);
            timerCheckbox.SetState(App.PlayerDataManager.ShowTimer);
            App.AudioManager.musicPlayer.PlayMusic(menuMusic);
            App.TimerManager.HideTimer();
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
            if (App.PlayerDataManager.HasPlayedOpeningDialogue) {
                App.SceneManager.LoadScene(SceneConstants.Game);
            } else {
                App.SceneManager.LoadScene(SceneConstants.OpeningDialogue);
            }
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
        
        public void SetMasterLevel(float value) {
            if (_freeze) return;
            App.PlayerDataManager.MasterLevel = value;
            App.AudioManager.SetMasterVolume(value);

            masterSlider.value = value;
        }
        
        public void SetMusicLevel(float value) {
            if (_freeze) return;
            
            App.PlayerDataManager.MusicLevel = value;
            App.AudioManager.music.SetVolume(value);

            musicSlider.value = value;
        }
        
        public void SetSFXLevel(float value) {
            if (_freeze) return;
            
            App.PlayerDataManager.SFXLevel = value;
            App.AudioManager.sfx.SetVolume(value);

            sfxSlider.value = value;
        }
        
        public void SetTimer(bool value) {
            if (_freeze) return;
            
            App.PlayerDataManager.ShowTimer = value;
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