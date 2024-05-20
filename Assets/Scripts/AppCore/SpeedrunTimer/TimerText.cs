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
            string formattedTime = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}.{timeSpan.Milliseconds / 10:00}";
            _text.text = formattedTime;
        }
        
        internal void Show(bool show) {
            _text.enabled = show;
        }
    }
}