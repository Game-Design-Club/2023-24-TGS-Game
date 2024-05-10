using System.Collections;

using Game.GameManagement;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.NightLevels.Spikes {
    public class Spike : MonoBehaviour { // Spike that turns on and off periodically
        [SerializeField] private GameObject spikesOn;
        [SerializeField] private GameObject spikesOff;
        [SerializeField] private float offDuration = 1f;
        [SerializeField] private float onDuration = 1f;
        [FormerlySerializedAs("delay")] [SerializeField] private float startDelay = 0f;

        // Unity functions
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
            GameManagerEvents.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
            GameManagerEvents.OnLevelOver -= OnLevelOver;
        }
        
        // Private functions
        private void OnLevelStart() {
            StartCoroutine(SpikeRoutine());
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private IEnumerator SpikeRoutine() {
            spikesOn.SetActive(true);
            spikesOff.SetActive(false);
            yield return new WaitForSeconds(startDelay);
            // Main loop
            while (true) {
                spikesOn.SetActive(true);
                spikesOff.SetActive(false);
                yield return new WaitForSeconds(onDuration);
                spikesOn.SetActive(false);
                spikesOff.SetActive(true);
                yield return new WaitForSeconds(offDuration);
            }
        }
    }
}
