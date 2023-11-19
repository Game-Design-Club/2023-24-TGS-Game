using UnityEngine;

namespace AppCore.SceneManager {
    public class SceneManager : MonoBehaviour{
        public void LoadScene(string sceneName, bool fade = false) {
            if (!Application.CanStreamedLevelBeLoaded(sceneName)) { // Check if scene exists
                Debug.LogError("Scene " + sceneName + " does not exist" +
                               "\n" +
                               "Check that the scene name is correct and that the scene is added to the build settings.");
                return;
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        public void ReloadScene(bool fade = false) {
            LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, fade);
        }
    }
}