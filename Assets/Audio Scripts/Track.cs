using UnityEngine;

namespace Audio_Scripts
{
    [System.Serializable]
    public struct Track
    {
        public AudioClip clip;
        [Range(0f, 2f)]
        public float clipVolume;
    }
}
