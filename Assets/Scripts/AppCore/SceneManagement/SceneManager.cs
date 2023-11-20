using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AppCore.SceneManagement {
    public class SceneManager : MonoBehaviour {
        
        public void ReloadScene(bool fade = true) {
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, fade);
        }
        
        public void LoadScene(string sceneName, bool fade = true) {
            if (!Application.CanStreamedLevelBeLoaded(sceneName)) {
                Debug.LogError("Scene " + sceneName + " does not exist" +
                               "\n" +
                               "Check that the scene name is correct and that the scene is added to the build settings.");
                return;
            }
            StartCoroutine(LoadSceneWithFade(sceneName, fade));
        }

        private IEnumerator LoadSceneWithFade(string sceneName, bool fade) {
            if (fade) {
                App.Instance.fadeManager.FadeIn();
            }
    
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            
            yield return new WaitForSeconds(App.Instance.fadeManager.transitionPeriod);
            
            asyncLoad.allowSceneActivation = true;
            
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) {
                // float currentProgress = asyncLoad.progress; -- could be used to make a progress bar
                yield return null;
            }

            if (fade) {
                App.Instance.fadeManager.FadeOut();
            }
        }
    }
}