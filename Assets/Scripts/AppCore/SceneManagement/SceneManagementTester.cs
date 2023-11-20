using System.Collections;

using Constants;

using UnityEngine;

namespace AppCore.SceneManagement
{
    public class SceneManagementTester : TesterScript.Tester
    {
        [ContextMenu(itemName: "Load Test Scene 1")]
        private void LoadScene1() {
            App.Instance.sceneManager.LoadScene(SceneConstants.TestScene1);
            DebugLog($"Scene 1 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 2")]
        private void LoadScene() {
            App.Instance.sceneManager.LoadScene(SceneConstants.TestScene2);
            DebugLog($"Scene 2 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Reload Scene")]
        private void ReloadScene() {
            App.Instance.sceneManager.ReloadScene();
            DebugLog($"Scene Reloaded. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 1 No Fade")]
        private void LoadScene1WithFade() {
            App.Instance.sceneManager.LoadScene(SceneConstants.TestScene1, false);
            DebugLog($"Scene 1 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
        
        [ContextMenu(itemName: "Load Test Scene 2 No Fade")]
        private void LoadSceneWithFade() {
            App.Instance.sceneManager.LoadScene(SceneConstants.TestScene2, false);
            DebugLog($"Scene 2 Loading. Current Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
    }
}
