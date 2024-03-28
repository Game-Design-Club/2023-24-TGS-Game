using UnityEngine;
using UnityEngine.Audio;

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
    
    }
}
