using UnityEngine;

namespace AppCore.AudioManagement {
    [System.Serializable]
    public class SoundPackage {
        [SerializeField] public AudioClip clip;
        [SerializeField] public float pitchAdjustment = 0;
        [SerializeField] public float pitchRandomness = 0;
        [SerializeField] public float volumeAdjustment = 0;
        [SerializeField] public float spatialBlend = 0;
        [SerializeField] public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [SerializeField] public float minDistance = 1;
        [SerializeField] public float maxDistance = 500;
        [SerializeField] public bool randomStartPos = false;
        
        public SoundPackage(AudioClip clip) {
            this.clip = clip;
        }
    }
}