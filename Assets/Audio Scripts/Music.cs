using UnityEngine;
using UnityEngine.Serialization;

namespace Audio_Scripts
{
    [System.Serializable]
    public class Music
    {
        [SerializeField] public string name;
        [SerializeField] private Track[] tracks;
        internal AudioSource[] Sources;
        public void Initiate()
        {
            Sources = new AudioSource[tracks.Length];
            for (int i = 0 ; i < tracks.Length ; i++)
            {
                Track track = tracks[i];
                AudioSource source = AudioManager.Instance.music.AddClip(track.clip);
                source.clip = track.clip;
                source.volume = track.clipVolume;
                source.loop = true;
                Sources[i] = source;
            }
        }

        public void Play()
        {
            foreach (AudioSource source in Sources)
            {
                source.Play();
            }
        }
        
        public void Stop()
        {
            foreach (AudioSource source in Sources)
            {
                source.Stop();
            }
        }

        public void SetTime(float time)
        {
            foreach (AudioSource source in Sources)
            {
                source.time = time;
            }
        }

        public float GetTime()
        {
            return Sources[0].time;
        }
    }
}
