using AppCore;

using Constants;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        private bool _freeze = false;
        
        public void StartGame() {
            if (_freeze) {
                return;
            }
            _freeze = true;
            App.Instance.sceneManager.LoadScene(SceneConstants.Game);
        }
        
        public void QuitGame() {
            if (_freeze) {
                return;
            }
            _freeze = true;
            Application.Quit();
        }
    }
}