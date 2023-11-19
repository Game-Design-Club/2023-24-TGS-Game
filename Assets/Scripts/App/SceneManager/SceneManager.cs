using UnityEngine;

namespace App.Scene {
    public class SceneManager : MonoBehaviour{
        public bool LoadScene(string sceneName){
            if (!Application.CanStreamedLevelBeLoaded(sceneName)) { // Check if scene exists
                Debug.LogError("Scene " + sceneName + " does not exist");
                return false;
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            return true;
        }
    }
}