using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.AudioManagement {
    public class MusicPlayer : MonoBehaviour { // This class is used to consolidate the music playing in the game, as well as further control over adaptive music
        [SerializeField] private Music mainMenuMusic;
        [SerializeField] private Music gameMusic;
        [SerializeField] private float fadeTime = 1f;
        
        [SerializeField] private Music currentMusic;
        
        private void Awake() {
            if (mainMenuMusic == null) {
                Debug.LogWarning("Main menu music is not set");
            }
            if (gameMusic == null) {
                Debug.LogWarning("Game music is not set");
            }
        }
        
        public void PlayMainMenuMusic() {
            if (currentMusic == mainMenuMusic) {
                Debug.LogWarning("Tried to play main menu music while it was already playing");
                return;
            }
            if (currentMusic == null) {
                App.AudioManager.music.FadeIn(mainMenuMusic, fadeTime);
            } else {
                App.AudioManager.music.FadeIntoCurrentTime(currentMusic, mainMenuMusic, fadeTime);
            }
            currentMusic = mainMenuMusic;
        }
        
        public void PlayGameMusic() {
            if (currentMusic == gameMusic) {
                Debug.LogWarning("Tried to play game music while it was already playing");
                return;
            }
            
            if (currentMusic == null) {
                App.AudioManager.music.FadeIn(gameMusic, fadeTime);
            } else {
                App.AudioManager.music.FadeIntoCurrentTime(currentMusic, gameMusic, fadeTime);
            }
            currentMusic = gameMusic;
        }
    }
}