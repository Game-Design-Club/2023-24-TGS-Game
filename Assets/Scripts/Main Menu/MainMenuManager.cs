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
        
        [SerializeField] private Checkbox sfxToggle;
        [SerializeField] private Checkbox musicToggle;
        
        private bool _freeze = false;
        
        // Unity functions
        private void Start() {
            SetSFXToggle(App.Instance.playerDataManager.AreSFXOn);
            SetMusicToggle(App.Instance.playerDataManager.IsMusicOn);
            App.Instance.audioManager.musicPlayer.PlayMainMenuMusic();
        }

        // Public functions
        public void StartGame() {
            if (_freeze) return;
            _freeze = true;
            App.Instance.sceneManager.LoadScene(SceneConstants.Game);
        }

        public void ShowCredits() { 
            if (_freeze) return;
            
            _freeze = true;
            App.Instance.sceneManager.LoadScene(SceneConstants.Credits);
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
        
        public void EraseProgress() {
            if (_freeze) return;
            
            App.Instance.playerDataManager.EraseLevelProgress();
        }
        
        public void SetSFXToggle(bool value) {
            if (_freeze) return;
            
            App.Instance.audioManager.sfx.SetVolume(value ? 1 : 0);
            App.Instance.playerDataManager.AreSFXOn = value;
            
            sfxToggle.SetState(value);
        }
        
        public void SetMusicToggle(bool value) {
            if (_freeze) return;
            
            App.Instance.playerDataManager.IsMusicOn = value;
            App.Instance.audioManager.music.Mute(!value);
            
            musicToggle.SetState(value);
        }
        
        public void ShowOptions() {
            if (_freeze) return;
            
            eventSystemManager.SetSelectedGameObject(defaultOptionsButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.ShowOptions);
        }
        
        public void HideOptions() {
            if (_freeze) return;
            
            eventSystemManager.SetSelectedGameObject(defaultButton);
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.HideOptions);
        }
    }
}