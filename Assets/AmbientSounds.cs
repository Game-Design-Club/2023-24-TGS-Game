using System.Collections;
using System.Collections.Generic;

using AppCore;
using AppCore.AudioManagement;

using UnityEngine;

public class AmbientSounds : MonoBehaviour {
    [SerializeField] private SoundPackage[] ambienceSounds;
    
    // Start is called before the first frame update
    void Start() {
        foreach (SoundPackage soundPackage in ambienceSounds) {
            App.AudioManager.PlaySFX(soundPackage, parent: transform);
        }
    }
}
