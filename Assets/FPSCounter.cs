using TMPro;

using UnityEngine;

public class FPSCounter : MonoBehaviour {
    private TextMeshProUGUI _text;
    private int c = 0;
    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update() {
        c++;
        if (c % 100 != 0) return;
        _text.text = $"FPS: {(int)(1 / Time.deltaTime)}";
    }
}
