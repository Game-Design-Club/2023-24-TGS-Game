using UnityEngine;
using UnityEngine.Serialization;

namespace AppCore.AudioManagement {
    public class MusicPlayer : MonoBehaviour {
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
                App.Instance.audioManager.music.FadeIn(mainMenuMusic, fadeTime);
                Debug.Log("Fading in main menu music");
            } else {
                App.Instance.audioManager.music.FadeIntoCurrentTime(currentMusic, mainMenuMusic, fadeTime);
                Debug.Log("Fading into current time main menu music");
            }
            currentMusic = mainMenuMusic;
        }
        
        public void PlayGameMusic() {
            if (currentMusic == gameMusic) {
                Debug.LogWarning("Tried to play game music while it was already playing");
                return;
            }
            
            if (currentMusic == null) {
                App.Instance.audioManager.music.FadeIn(gameMusic, fadeTime);
                Debug.Log("Fading in game music");
            } else {
                App.Instance.audioManager.music.FadeIntoCurrentTime(currentMusic, gameMusic, fadeTime);
                Debug.Log("Fading into current time game music");
            }
            currentMusic = gameMusic;
        }
    }
}