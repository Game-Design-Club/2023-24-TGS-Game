using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.AudioManagement {
    public class MusicPlayer : MonoBehaviour { // This class is used to consolidate the music playing in the game, as well as further control over adaptive music
        [SerializeField] private float fadeTime = 1f;
        
        private Music currentMusic;
        
        private void Awake() {
            if (App.AudioManager == null || App.AudioManager.music == null) {
                Debug.LogWarning("AudioManager or AudioManager.music is not set");
            }
        }
        
        public void PlayMusic(Music newMusic) {
            if (newMusic == null) {
                Debug.LogWarning("Tried to play music, but new music is not set");
                return;
            }
            
            if (currentMusic == newMusic) {
                return;
            }
            
            if (currentMusic == null) {
                App.AudioManager.music.FadeIn(newMusic, fadeTime);
            } else {
                App.AudioManager.music.FadeIntoCurrentTime(currentMusic, newMusic, fadeTime);
            }
            currentMusic = newMusic;
        }
    }
}