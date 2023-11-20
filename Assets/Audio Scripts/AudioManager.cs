using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio_Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public MusicManager music;
        public SFXManager sfx;
        
        //constant varriables used to access different groups in the audio mixer
        private const string MASTER_VOLUME = "MasterVolume";
    
        //the audio mixer and its groups
        [SerializeField] private AudioMixer mixer;
    
        //Volume of groups
        [Range(0f, 1f)] public float masterVolume = 1;

        private void Awake()
        {
            mixer.SetFloat(MASTER_VOLUME, ConvertToDecibels(masterVolume));
        }

        //converts a volume level from 0f-1f to corresponding value in decibels
        private float ConvertToDecibels(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }
    
        //***** Master *****
    
        public void MuteMaster(bool mute)
        {
            if (mute)
            {
                mixer.SetFloat(MASTER_VOLUME, ConvertToDecibels(0f));
            }
            else
            {
                mixer.SetFloat(MASTER_VOLUME, masterVolume);
            }
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = ConvertToDecibels(volume);
            mixer.SetFloat(MASTER_VOLUME, ConvertToDecibels(masterVolume));
        }
    
    }
}
