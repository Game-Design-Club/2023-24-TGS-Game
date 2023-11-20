using System;
using System.Collections;
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
        internal List<AudioSource> Sources = new List<AudioSource>();
        internal AudioMixerGroup CurrentGroup = null;
        private bool sourcesAdded = false;

        private void OnValidate()
        {
            if (Application.isPlaying && sourcesAdded)
            {
                if (Sources.Count < tracks.Length)
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
                        Sources.Insert(i, newSource);
                        newSource.outputAudioMixerGroup = CurrentGroup;
                        newSource.time = GetTime();
                        newSource.Play();
                    }
                    ReSink();
                }else if (Sources.Count > tracks.Length)
                {
                    for (int i = 0; i < Sources.Count ; i++)
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
                        Sources.RemoveAt(i);
                    }
                    ReSink();
                }
                
                for (int i = 0; i < tracks.Length && i < Sources.Count; i++)
                {
                    Track track = tracks[i];
                    AudioSource source = Sources[i];
                    source.volume = track.clipVolume;
                    if (source.clip != track.clip)
                    {
                        source.Stop();
                        source.clip = track.clip;
                        ReSink();
                        source.Play();
                    }
                }
            }
        }

        internal void AddSources()
        {
            Sources.Clear();
            for (int i = 0; i < tracks.Length; i++)
            {
                Track track = tracks[i];
                AudioSource source = CreateSource(track);
                Sources.Add(source);
            }

            sourcesAdded = true;
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

            sourcesAdded = false;
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
    }
}
