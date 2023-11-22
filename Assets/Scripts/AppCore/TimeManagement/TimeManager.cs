using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace AppCore.TimeManagement {
    public class TimeManager : MonoBehaviour{
        public void SetTimeScale(float timeScale, bool stopOthers = false) {
            Time.timeScale = timeScale;
            if (stopOthers) {
                StopAllCoroutines();
            }
        }

        public void SlowTime(float timeScale, float duration = 1f) {
            Time.timeScale = timeScale;
            StartCoroutine(ResetTimeScale(duration));
        }

        private IEnumerator ResetTimeScale(float duration) {
            yield return new WaitForSeconds(duration);
            Time.timeScale = 1f;
        }
    }
}