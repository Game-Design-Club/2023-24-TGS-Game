using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore.SceneManagement {
    public class SceneManager : MonoBehaviour {
        
        public void ReloadScene(bool fade = true) {
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, fade);
        }
        
        public void LoadScene(int sceneIndex, bool fade = true) {
            if (!Application.CanStreamedLevelBeLoaded(sceneIndex)) {
                Debug.LogError("Scene " + sceneIndex + " does not exist" +
                               "\n" +
                               "Check that the scene name is correct and that the scene is added to the build settings.");
                return;
            }
            StartCoroutine(LoadSceneWithFade(sceneIndex, fade));
        }

        private IEnumerator LoadSceneWithFade(int sceneIndex, bool fade) {
            if (fade) {
                App.Instance.transitionManager.FadeIn();
            }

            if (fade) {
                yield return new WaitForSecondsRealtime(App.Instance.transitionManager.fadeTime);
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

            if (fade) {
                App.Instance.transitionManager.FadeOut();
            }
        }
    }
}