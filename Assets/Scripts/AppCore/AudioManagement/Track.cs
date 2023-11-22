using UnityEngine;

namespace AppCore.AudioManagement
{
    [System.Serializable]
    public struct Track
    {
        public AudioClip clip;
        [Range(0f, 2f)]
        public float clipVolume;
    }
}
