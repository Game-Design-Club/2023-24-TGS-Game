using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

using Random = UnityEngine.Random;

namespace AppCore.AudioManagement
{
    public class SFXManager : MonoBehaviour { // Used to manage the sound effects in the game
        //constant variables used to access different groups in the audio mixer
        private const string SfxVolume = "SFXVolume";

        //the audio mixer and its groups
        private AudioMixer _mixer = null;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [Range(0f, 1f)] private float sfxVolume = 1;

        private readonly LinkedList<GameObject> _currentSoundEffects = new LinkedList<GameObject>();

        private void Awake() {
            _mixer = sfxGroup.audioMixer;
        }

        private void OnValidate() {
            if (_mixer == null) return;
            _mixer.SetFloat(SfxVolume, AudioManager.ConvertToDecibels(sfxVolume));
        }

        //****** SFX ********
        public void Play(SoundPackage s, Vector2 position = default, System.Func<bool> stopCondition = null, Transform parent = null) {
            Play(s.clip, s.volumeAdjustment, s.pitchAdjustment, s.pitchRandomness, 
                s.spatialBlend, s.minDistance, s.maxDistance,
                s.randomStartPos,
                position, stopCondition, parent);
        }
        
        public void Play(AudioClip clip, float volumeAdjustment = 0, float pitchAdjustment = 0f, float randomAdjustment = 0f,
            float spatialBlend = 0, float minDistance = 1, float maxDistance = 500,
            bool randomStartPos = false,
            Vector2 position = default, System.Func<bool> stopCondition = null, Transform parent = null) {
            
            GameObject soundObject = new GameObject("SFX");
            soundObject.transform.position = position;
            soundObject.transform.SetParent(parent == null ? transform : parent);
            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.time = randomStartPos ? Random.Range(0, clip.length) : 0;
            source.volume += volumeAdjustment;
            source.pitch += pitchAdjustment + Random.Range(-randomAdjustment, randomAdjustment);
            source.spatialBlend = spatialBlend;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            source.outputAudioMixerGroup = sfxGroup;

            _currentSoundEffects.AddLast(soundObject);
            if (parent != null) {
                // Used especially for ambience noise
                if (stopCondition != null) {
                    Debug.LogWarning("Parented sound effects will ignore stop conditions - once it's parented by someone else it's their responsibility to deal.");
                }
                source.loop = true;
                source.Play();
            } else {
                if (stopCondition == null) {
                    StartCoroutine(PlaySourceAndRemove(source, soundObject));
                } else {
                    source.loop = true;
                    StartCoroutine(PlaySourceRepeatingAndRemove(source, soundObject, stopCondition));
                }
            }
        }
        private IEnumerator PlaySourceAndRemove(AudioSource source, GameObject soundObject) {
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            _currentSoundEffects.Remove(soundObject);
            Destroy(soundObject, 0.1f);
        }
        
        private IEnumerator PlaySourceRepeatingAndRemove(AudioSource source, GameObject soundObject, System.Func<bool> stopCondition) {
            source.Play();
            while (!stopCondition()) {
                yield return null;
            }
            source.Stop();
            _currentSoundEffects.Remove(soundObject);
            Destroy(soundObject, 0.1f);
        }
        
        //Mutes or un-mutes the sfx audio group
        public void Mute(bool mute) {
            _mixer.SetFloat(SfxVolume, mute ? AudioManager.ConvertToDecibels(0f) : AudioManager.ConvertToDecibels(sfxVolume));
        }

        //sets volume for sfx audio group
        public void SetVolume(float volume) {
            sfxVolume = volume;
            _mixer.SetFloat(SfxVolume, AudioManager.ConvertToDecibels(sfxVolume));
        }

        //Stops and deletes all sfx
        public void StopAll()
        {
            foreach (GameObject source in _currentSoundEffects)
            {
                source.GetComponent<AudioSource>().Stop();
                Destroy(source);
            }
            StopAllCoroutines();

            _currentSoundEffects.Clear();
        }
    }
}
