using UnityEngine;

namespace AppCore.AudioManagement {
    public class SoundAmbience : MonoBehaviour {
        [SerializeField] private SoundPackage[] soundPackages;
        [SerializeField] private AudioClip[] clips;

        private void Start() {
            foreach (var soundPackage in soundPackages) {
                App.Instance.audioManager.PlaySFX(soundPackage, parent: transform);
            }

            foreach (var clip in clips) {
                App.Instance.audioManager.PlaySFX(clip, parent: transform);
            }
        }
    }
}
