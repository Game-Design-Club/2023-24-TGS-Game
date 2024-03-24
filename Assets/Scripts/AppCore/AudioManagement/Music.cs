using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

namespace AppCore.AudioManagement { // This class is used to store the music objects in the game
    [CreateAssetMenu(fileName = "Music", menuName = "Music", order = 1)]
    public class Music : ScriptableObject
    {
        [SerializeField] private Track[] tracks;
        internal readonly List<AudioSource> Sources = new List<AudioSource>();
        internal AudioMixerGroup CurrentGroup = null;
        private bool _sourcesAdded = false;

        private void OnValidate()
        {
            
            //Check to make sure the music is currently activated
            if (!Application.isPlaying || !_sourcesAdded) return;
            
            if (Sources.Count < tracks.Length) //track added
            {
                //find new track and add source
                for (int i = 0; i < tracks.Length ; i++)
                {
                    Track track = tracks[i];
                    
                    if (track.clip == null) continue;
                    
                    bool found = Sources.Any(source => source.clip == track.clip);

                    if (found) continue;
                    
                    AudioSource newSource = CreateSource(track);
                    Sources.Insert(i, newSource);
                    newSource.outputAudioMixerGroup = CurrentGroup;
                    newSource.Play();
                }
                ReSink();
            }else if (Sources.Count > tracks.Length) // track removed
            {
                //finds removed track and removes source
                for (int i = 0; i < Sources.Count ; i++)
                {
                    AudioSource source = Sources[i];

                    bool found = tracks.Any(track => source.clip == track.clip);

                    if (found) continue;
                    
                    App.Instance.audioManager.music.RemoveSource(source);
                    Sources.RemoveAt(i);
                }
                ReSink();
            }
            
            //updating volume and clips
            for (int i = 0; i < tracks.Length && i < Sources.Count; i++)
            {
                Track track = tracks[i];
                AudioSource source = Sources[i];
                source.volume = track.clipVolume;
                
                //clip changed
                if (source.clip == track.clip) continue;
                
                source.Stop();
                source.clip = track.clip;
                ReSink();
                source.Play();
            }
            
        }

        //Adds new sources for each tracks
        internal void AddSources()
        {
            Sources.Clear();
            foreach (var track in tracks)
            {
                AudioSource source = CreateSource(track);
                Sources.Add(source);
            }

            _sourcesAdded = true;
        }

        //Creates a new source and integrates track into it
        private AudioSource CreateSource(Track track)
        {
            AudioSource source = App.Instance.audioManager.music.GetNewSource();
            source.playOnAwake = false;
            source.clip = track.clip;
            source.volume = track.clipVolume;
            source.loop = true;
            return source;
        }
        
        //Removes all sources from scene
        internal void RemoveSources()
        {
            foreach (AudioSource source in Sources)
            {
                App.Instance.audioManager.music.RemoveSource(source);
            }

            _sourcesAdded = false;
        }

        //Plays all sources
        internal void Play()
        {
            foreach (AudioSource source in Sources)
            {
                source.Play();
            }
        }
        
        //stops all sources
        internal void Stop()
        {
            foreach (AudioSource source in Sources)
            {
                source.Stop();
            }
        }

        //sets the time of all sources
        internal void SetTime(float time)
        {
            foreach (AudioSource source in Sources)
            {
                if (time > source.clip.length)
                {
                    Debug.LogWarning("Tried to set the time of a clip outside its length");
                    continue;
                }
                source.time = time;
            }
        }

        //gets the current time of the first source 
        internal float GetTime()
        {
            return Sources[0].time;
        }

        //Sets all of the sources times equal to the first source
        private void ReSink()
        {
            float time = GetTime();
            foreach (AudioSource source in Sources)
            {
                source.time = time;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
