using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrewSlider : MonoBehaviour {
    [SerializeField] private Transform screw;
    
    private Slider _slider;

    private void Awake() {
        _slider = GetComponent<Slider>();
        UpdateRotation(_slider.value);
        _slider.onValueChanged.AddListener(UpdateRotation);
    }

    private void UpdateRotation(float percent) {
        screw.localRotation = Quaternion.Euler(0, 0, 360 - (360 * percent));
    }
}
