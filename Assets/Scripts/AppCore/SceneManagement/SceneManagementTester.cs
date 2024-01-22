using Tools.TesterScript;

using UnityEngine;

namespace AppCore.SceneManagement
{
    public class SceneManagementTester : Tester
    {
        [ContextMenu(itemName: "Load Test Scene 1")]
        private void LoadScene1() {
            DebugLog($"Scene 1 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 2")]
        private void LoadScene() {
            DebugLog($"Scene 2 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Reload Scene")]
        private void ReloadScene() {
            App.Instance.sceneManager.ReloadScene();
            DebugLog($"Scene Reloaded. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 1 No Fade")]
        private void LoadScene1WithFade() {
            DebugLog($"Scene 1 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 2 No Fade")]
        private void LoadSceneWithFade() {
            DebugLog($"Scene 2 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
    }
}
