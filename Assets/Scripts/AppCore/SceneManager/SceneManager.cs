using UnityEngine;

namespace AppCore.SceneManager {
    public class SceneManager : MonoBehaviour{
        public bool LoadScene(string sceneName, bool fade = false) {
            if (!Application.CanStreamedLevelBeLoaded(sceneName)) { // Check if scene exists
                Debug.LogError("Scene " + sceneName + " does not exist\nCheck that the scene name is correct and that the scene is added to the build settings.");
                return false;
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            return true;
        }
    }
}