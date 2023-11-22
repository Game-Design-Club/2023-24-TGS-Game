using UnityEngine;
using UnityEngine.Audio;

namespace AppCore.AudioManagement
{
    public class AudioManager : MonoBehaviour
    {
        public MusicManager music;
        public SFXManager sfx;
        
        //constant variables used to access different groups in the audio mixer
        private const string MasterVolume = "MasterVolume";
    
        //the audio mixer and its groups
        [SerializeField] private AudioMixer mixer;
    
        //Volume of groups
        [Range(0f, 1f)] public float masterVolume = 1;

        public static AudioManager Instance;
        private void Awake()
        {
            Instance = this;
            mixer.SetFloat(MasterVolume, ConvertToDecibels(masterVolume));
        }
        
        private void OnValidate()
        {
            mixer.SetFloat(MasterVolume, ConvertToDecibels(masterVolume));
        }

        //converts a volume level from 0f-1f to corresponding value in decibels
        internal static float ConvertToDecibels(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }
    
        //***** Master *****
    
        public void MuteMaster(bool mute)
        {
            mixer.SetFloat(MasterVolume, mute ? ConvertToDecibels(0f) : ConvertToDecibels(masterVolume));
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = ConvertToDecibels(volume);
            mixer.SetFloat(MasterVolume, ConvertToDecibels(masterVolume));
        }
    
    }
}
