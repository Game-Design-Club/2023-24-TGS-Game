using System.Collections;
using System.Collections.Generic;

using AppCore;
using AppCore.AudioManagement;

using UnityEngine;

public class MusicTransitioner : MonoBehaviour {
    [SerializeField] private Music music;
    
    private void Start() {
        App.AudioManager.musicPlayer.PlayMusic(music);
    }
}
