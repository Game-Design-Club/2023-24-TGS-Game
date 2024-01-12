using System.Collections;

using AppCore;

using Constants;

using UnityEngine;
using UnityEngine.UI;

namespace Preload
{
    public class Preloader : MonoBehaviour {
        [SerializeField] private float preWaitTime = 1f;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float waitTime = 2f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float postWaitTime = 1f;
        [SerializeField] private Image fadeCanvas;
    
        private void Awake() {
            StartCoroutine(Preload());
        }
    
        private IEnumerator Preload() {
            yield return new WaitForSecondsRealtime(preWaitTime);
            
            float t = 0;
            while (t < fadeOutTime) {
                t += Time.deltaTime;
                fadeCanvas.color = Color.LerpUnclamped(Color.black, Color.clear, t / fadeOutTime);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(waitTime);

            t = 0;
            while (t < fadeInTime) {
                t += Time.deltaTime;
                fadeCanvas.color = Color.LerpUnclamped(Color.clear, Color.black, t / fadeInTime);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(postWaitTime);
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu, false);
        }
    }
}
