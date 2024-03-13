using AppCore;

using Tools.Constants;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
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
                UnityEditor.EditorApplication.isPlaying = false;
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
        }
        
        public void SetMusicToggle(bool value) {
            if (_freeze) return;
            
            App.Instance.audioManager.music.musicVolume = value ? 1 : 0;
        }
    }
}