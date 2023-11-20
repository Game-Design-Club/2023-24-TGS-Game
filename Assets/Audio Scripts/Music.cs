using UnityEngine;
namespace Audio_Scripts
{
    [System.Serializable]
    public class Music
    {
        [SerializeField]
        private GameObject gameObject;
        public AudioClip[] clips;
        internal AudioSource[] sources;

        Music()
        {
            if (clips == null)
            {
                Debug.LogError("clips is null");
            }
            if (gameObject == null)
            {
                Debug.LogError("gameObject is null");
            }
            int p = 0;
            sources = new AudioSource[clips.Length];
            for (int i = 0 ; i < clips.Length ; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
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
