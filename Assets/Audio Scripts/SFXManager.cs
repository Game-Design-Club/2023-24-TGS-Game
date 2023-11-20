using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio_Scripts
{
    public class SFXManager : MonoBehaviour
    {
        //constant varriables used to access different groups in the audio mixer
        private const string SFX_VOLUME = "SFXVolume";

        //the audio mixer and its groups
        private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup sfxGroup;

        //Volume of groups
        [Range(0f, 1f)] public float sfxVolume = 1;

        private LinkedList<AudioSource> _currentSoundEffects;

        private void Awake()
        {
            mixer = sfxGroup.audioMixer;
            
            _currentSoundEffects = new LinkedList<AudioSource>();
        }


        //converts a volume level from 0f-1f to corresponding value in decibels
        private float ConvertToDecibels(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f);
            return Mathf.Log10(volume) * 20;
        }

        //****** SFX ********

        public void Play(AudioSource source)
        {
            source.outputAudioMixerGroup = sfxGroup;
            
            _currentSoundEffects.AddLast(source);

            StartCoroutine(PlaySourceAndRemove(source));
        }

        private IEnumerator PlaySourceAndRemove(AudioSource source)
        {
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            _currentSoundEffects.Remove(source);
        }

        //Mutes or un-mutes the sfx audio group
        public void Mute(bool mute)
        {
            if (mute)
            {
                mixer.SetFloat(SFX_VOLUME, ConvertToDecibels(0f));
            }
            else
            {
                mixer.SetFloat(SFX_VOLUME, sfxVolume);
            }
        }

        //sets volume for sfx audio group
        public void SetVolume(float volume)
        {
            sfxVolume = ConvertToDecibels(volume);
            mixer.SetFloat(SFX_VOLUME, ConvertToDecibels(sfxVolume));
        }

        //Stops and deletes all sfx
        public void StopAll()
        {
            foreach (AudioSource source in _currentSoundEffects)
            {
                source.Stop();
                Destroy(source);
            }
            StopAllCoroutines();

            _currentSoundEffects.Clear();
        }
    }
}
