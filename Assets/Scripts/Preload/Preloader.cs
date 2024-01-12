using AppCore;

using Constants;

using UnityEngine;

namespace Preload
{
    public class Preloader : MonoBehaviour {
        private void Start() {
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu, false);
        }
    }
}
