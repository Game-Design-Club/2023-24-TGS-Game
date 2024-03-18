using AppCore;

using Game.GameManagement.UIManagement;

using Tools.Constants;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        [SerializeField] private Animator menuAnimator;
        
        private bool _freeze = false;
        
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
            
            Debug.Log("Progress erased");
        }
        
        public void SetSFXToggle(bool value) {
            if (_freeze) return;
            
            App.Instance.audioManager.sfx.sfxVolume = value ? 1 : 0;
            
            Debug.Log("SFX: " + App.Instance.audioManager.sfx.sfxVolume);
        }
        
        public void SetMusicToggle(bool value) {
            if (_freeze) return;
            
            App.Instance.audioManager.music.musicVolume = value ? 1 : 0;
            
            Debug.Log("Music: " + App.Instance.audioManager.music.musicVolume);
        }
        
        public void ShowOptions() {
            if (_freeze) return;
            
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.ShowOptions);
        }
        
        public void HideOptions() {
            if (_freeze) return;
            
            menuAnimator.SetTrigger(AnimationConstants.MainMenu.HideOptions);
        }
    }
}