using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore.SceneManagement {
    public class SceneManager : MonoBehaviour { // Manages scene loading and transitions
        
        public void ReloadScene(bool fade = true) { // Reloads the current scene
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, fade);
        }
        
        public void LoadScene(int sceneIndex, bool fade = true) { // Loads a scene by index
            if (!Application.CanStreamedLevelBeLoaded(sceneIndex)) {
                Debug.LogError("Scene " + sceneIndex + " does not exist" +
                               "\n" +
                               "Check that the scene name is correct and that the scene is added to the build settings.");
                return;
            }
            StartCoroutine(LoadSceneWithFade(sceneIndex, fade));
        }

        private IEnumerator LoadSceneWithFade(int sceneIndex, bool fade) { // Coroutine to load a scene with a fade transition
            if (fade) {
                App.Instance.transitionManager.FadeIn();
            }

            if (fade) {
                yield return new WaitForSecondsRealtime(App.Instance.transitionManager.fadeTime);
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            Time.timeScale = 1;
            if (fade) {
                App.Instance.transitionManager.FadeOut();
            }
        }
    }
}