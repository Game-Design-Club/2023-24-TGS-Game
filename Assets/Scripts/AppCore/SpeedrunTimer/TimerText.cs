using System;

using TMPro;

using UnityEngine;

namespace AppCore.SpeedrunTimer {
    public class TimerText : MonoBehaviour {
        TextMeshProUGUI _text;
        private void Awake() {
            _text = GetComponent<TextMeshProUGUI>();
        }

        internal void UpdateText(float time) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            _text.text = formattedTime;
        }
        
        internal void Show(bool show) {
            _text.enabled = show;
        }
    }
}