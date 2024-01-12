using AppCore;

using Constants;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        public void StartGame() {
            App.Instance.sceneManager.LoadScene(SceneConstants.Game);
        }
    }
}