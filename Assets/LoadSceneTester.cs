using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppCore;

public class LoadSceneTester : MonoBehaviour
{
    private void Start() {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene() {
        yield return new WaitForSeconds(1f);
        App.Instance.SceneManager.ReloadScene();
    }
}
