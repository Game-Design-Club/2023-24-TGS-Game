using System;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace AppCore.AudioManagement
{
    public class AudioManager : MonoBehaviour { // This class is used to manage the audio in the game
        [SerializeField] public MusicManager music;
        [SerializeField] public MusicPlayer musicPlayer;
        [SerializeField] public SFXManager sfx;
        
        //the audio mixer and its groups
        [SerializeField] private AudioMixer mixer;
    
        //Volume of groups
        [Range(0f, 1f)] public float masterVolume = 1;

        private void Awake() {
            mixer.SetFloat(MixerConstants.MasterVolume, ConvertToDecibels(masterVolume));
        }
        
        private void OnValidate() {
            mixer.SetFloat(MixerConstants.MasterVolume, ConvertToDecibels(masterVolume));
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //converts a volume level from 0f-1f to corresponding value in decibels
        internal static float ConvertToDecibels(float volume) {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }
    
        //***** Master *****
    
        public void MuteMaster(bool mute) {
            mixer.SetFloat(MixerConstants.MasterVolume, mute ? ConvertToDecibels(0f) : ConvertToDecibels(masterVolume));
        }

        public void SetMasterVolume(float volume) {
            masterVolume = ConvertToDecibels(volume);
            mixer.SetFloat(MixerConstants.MasterVolume, ConvertToDecibels(masterVolume));
        }
        
        public void PlaySFX(AudioClip clip, float volumeAdjustment = 0, float pitchAdjustment = 0f, float randomAdjustment = 0f,
            float spatialBlend = 0, float minDistance = 1, float maxDistance = 500,
            bool randomStartPos = false,
            Vector2 position = default, Func<bool> stopCondition = null, Transform parent = null) {
            
            sfx.Play(clip, volumeAdjustment, pitchAdjustment, randomAdjustment, spatialBlend, minDistance, maxDistance, randomStartPos, position, stopCondition, parent);
        }
        
        public void PlaySFX(SoundPackage soundPackage, Vector2 position = default, Func<bool> stopCondition = null, Transform parent = null) {
            sfx.Play(soundPackage, position, stopCondition, parent);
        }
        
        // Private functions
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            music.Mute(!App.Instance.playerDataManager.IsMusicOn);
            sfx.Mute(!App.Instance.playerDataManager.AreSFXOn);
        }
    }
}
