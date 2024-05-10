using UnityEngine;

namespace AppCore.AudioManagement
{
    [System.Serializable]
    public struct Track { // Stores the audio clip and volume for a track, many combine into a Music object
        public AudioClip clip;
        [Range(0f, 2f)]
        public float clipVolume;
    }
}
