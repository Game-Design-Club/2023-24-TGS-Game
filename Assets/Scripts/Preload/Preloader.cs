using AppCore;

using Tools.Constants;

using UnityEngine;

namespace Preload
{
    public class Preloader : MonoBehaviour { // Preloader runs first to load the main menu
        private void Start() {
            App.SceneManager.LoadScene(SceneConstants.MainMenu, false);
        }
    }
}
