using UnityEngine;

namespace Audio_Scripts
{
    public class Music
    {
        public AudioClip[] clips;
        internal AudioSource[] sources;

        Music(AudioClip[] clips)
        {
            sources = new AudioSource[clips.Length];
            for (int i = 0 ; i < clips.Length ; i++)
            {
                AudioSource source = new AudioSource();
                source.clip = clips[i];
                sources[i] = source;
            }
        }

        public void Play()
        {
            foreach (AudioSource source in sources)
            {
                source.Play();
            }
        }
        
        public void Stop()
        {
            foreach (AudioSource source in sources)
            {
                source.Stop();
            }
        }

        public void SetTime(float time)
        {
            foreach (AudioSource source in sources)
            {
                source.time = time;
            }
        }

        public float GetTime()
        {
            return sources[0].time;
        }
    }
}
