using System.Collections;
using UnityEngine;
using AppCore;

using Constants;

public class LoadSceneTester : MonoBehaviour
{
    private void Start() {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene() {
        yield return new WaitForSeconds(1f);
        App.Instance.sceneManager.LoadScene(SceneConstants.TestScene2, true);
    }
}
