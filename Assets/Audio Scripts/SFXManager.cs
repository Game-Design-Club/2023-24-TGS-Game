using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Audio_Scripts
{
    public class SFXManager : MonoBehaviour
    {
        //constant variables used to access different groups in the audio mixer
        private const string SfxVolume = "SFXVolume";

        //the audio mixer and its groups
        private AudioMixer _mixer = null;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [Range(0f, 1f)] public float sfxVolume = 1;

        private readonly LinkedList<AudioSource> _currentSoundEffects = new LinkedList<AudioSource>();

        private void Awake()
        {
            _mixer = sfxGroup.audioMixer;
        }

        private void OnValidate()
        {
            if (_mixer == null) return;
            _mixer.SetFloat(SfxVolume, AudioManager.ConvertToDecibels(sfxVolume));
        }

        //****** SFX ********
        
        public void Play(AudioClip clip)
        {
            //creates source
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = sfxGroup;
            
            _currentSoundEffects.AddLast(source);

            //plays sound and deletes source
            StartCoroutine(PlaySourceAndRemove(source));
        }
        
        public void PlayWithRandomPitchAdjustment(AudioClip clip, float maxPitchAdjustment)
        {
            //creates source
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            //adjusts pitch
            source.pitch += Random.Range(-maxPitchAdjustment, maxPitchAdjustment);
            source.outputAudioMixerGroup = sfxGroup;
            
            _currentSoundEffects.AddLast(source);

            //plays sound and deletes source
            StartCoroutine(PlaySourceAndRemove(source));
        }

        private IEnumerator PlaySourceAndRemove(AudioSource source)
        {
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            _currentSoundEffects.Remove(source);
            Destroy(source, 1);
        }

        //Mutes or un-mutes the sfx audio group
        public void Mute(bool mute)
        {
            _mixer.SetFloat(SfxVolume, mute ? AudioManager.ConvertToDecibels(0f) : AudioManager.ConvertToDecibels(sfxVolume));
        }

        //sets volume for sfx audio group
        public void SetVolume(float volume)
        {
            sfxVolume = volume;
            _mixer.SetFloat(SfxVolume, AudioManager.ConvertToDecibels(sfxVolume));
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
