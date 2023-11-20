using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Audio_Scripts
{
    [CreateAssetMenu(fileName = "Music", menuName = "Music", order = 1)]
    public class Music : ScriptableObject
    {
        [SerializeField] public string musicName = "Unnamed Music";
        [SerializeField] private Track[] tracks;
        internal AudioSource[] Sources = Array.Empty<AudioSource>();
        internal AudioMixerGroup CurrentGroup = null;

        private void OnValidate()
        {
            if (Application.isPlaying && CurrentGroup != null)
            {
                if (Sources.Length < tracks.Length)
                {
                    for (int i = 0; i < tracks.Length ; i++)
                    {
                        Track track = tracks[i];
                        
                        if (track.clip == null) continue;
                        
                        bool found = false;
                        foreach (AudioSource source in Sources)
                        {
                            if (source.clip == track.clip)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found) continue;
                        AudioSource newSource = CreateSource(track);
                        Sources = InsertAudioSourceAtIndex(Sources, newSource, i);
                        newSource.outputAudioMixerGroup = CurrentGroup;
                        newSource.time = GetTime();
                        newSource.Play();
                    }
                    ReSink();
                }else if (Sources.Length > tracks.Length)
                {
                    for (int i = 0; i < Sources.Length ; i++)
                    {
                        AudioSource source = Sources[i];

                        bool found = false;
                        foreach (Track track in tracks)
                        {
                            if (source.clip == track.clip)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found) continue;
                        
                        AudioManager.Instance.music.RemoveSource(source);
                        Sources = RemoveAudioSourceAtIndex(Sources, i);
                    }
                    ReSink();
                }
                
                for (int i = 0; i < tracks.Length && i < Sources.Length; i++)
                {
                    Track track = tracks[i];
                    AudioSource source = Sources[i];
                    source.volume = track.clipVolume;
                    if (source.clip != track.clip)
                    {
                        source.clip = track.clip;
                        ReSink();
                    }
                }
            }
        }

        internal void AddSources()
        {
            Sources = new AudioSource[tracks.Length];
            for (int i = 0; i < tracks.Length; i++)
            {
                Track track = tracks[i];
                AudioSource source = CreateSource(track);
                Sources[i] = source;
            }
        }

        private AudioSource CreateSource(Track track)
        {
            AudioSource source = AudioManager.Instance.music.AddClip(track.clip);
            source.playOnAwake = false;
            source.clip = track.clip;
            source.volume = track.clipVolume;
            source.loop = true;
            return source;
        }
        
        internal void RemoveSources()
        {
            foreach (AudioSource source in Sources)
            {
                AudioManager.Instance.music.RemoveSource(source);
            }
        }

        internal void Play()
        {
            foreach (AudioSource source in Sources)
            {
                source.Play();
            }
        }
        
        internal void Stop()
        {
            foreach (AudioSource source in Sources)
            {
                source.Stop();
            }
        }

        internal void SetTime(float time)
        {
            foreach (AudioSource source in Sources)
            {
                source.time = time;
            }
        }

        internal float GetTime()
        {
            return Sources[0].time;
        }

        private void ReSink()
        {
            float time = GetTime();
            foreach (AudioSource source in Sources)
            {
                source.time = time;
            }
        }
        
        private AudioSource[] InsertAudioSourceAtIndex(AudioSource[] originalArray, AudioSource newAudioSource, int index)
        {
            // Check if the index is within valid bounds
            if (index < 0 || index > originalArray.Length)
            {
                Debug.LogError("Index out of bounds!");
                return originalArray;
            }

            // Create a new array with increased size
            AudioSource[] newArray = new AudioSource[originalArray.Length + 1];

            // Copy elements before the specified index
            for (int i = 0; i < index; i++)
            {
                newArray[i] = originalArray[i];
            }

            // Insert the new AudioSource
            newArray[index] = newAudioSource;

            // Copy elements after the specified index
            for (int i = index + 1; i < newArray.Length; i++)
            {
                newArray[i] = originalArray[i - 1];
            }

            return newArray;
        }
        AudioSource[] RemoveAudioSourceAtIndex(AudioSource[] originalArray, int index)
        {
            // Check if the index is within valid bounds
            if (index < 0 || index >= originalArray.Length)
            {
                Debug.LogError("Index out of bounds!");
                return originalArray;
            }

            // Create a new array with reduced size
            AudioSource[] newArray = new AudioSource[originalArray.Length - 1];

            // Copy elements before the specified index
            for (int i = 0; i < index; i++)
            {
                newArray[i] = originalArray[i];
            }

            // Copy elements after the specified index
            for (int i = index; i < newArray.Length; i++)
            {
                newArray[i] = originalArray[i + 1];
            }

            return newArray;
        }
    }
}
