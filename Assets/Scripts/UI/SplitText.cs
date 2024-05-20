using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SplitText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI splitText;
    
        internal void UpdateSplitText(float time) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string formattedTime = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}.{timeSpan.Milliseconds / 10:00}";
            splitText.text = formattedTime;
        }

        internal void UpdateLevelText(int level)
        {
            string text = "Level " + level;
            levelText.text = text;
        }
    }
}
