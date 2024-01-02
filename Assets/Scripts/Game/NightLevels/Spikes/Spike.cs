using System;
using System.Collections;

using Game.GameManagement;

using UnityEngine;

namespace Game.NightLevels.Spikes {
    public class Spike : MonoBehaviour {
        [SerializeField] private GameObject spikesOn;
        [SerializeField] private GameObject spikesOff;
        [SerializeField] private float offDuration = 1f;
        [SerializeField] private float onDuration = 1f;

        // Unity functions
        private void OnEnable() {
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnLevelOver += OnLevelOver;
        }
        
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            GameManager.Instance.OnLevelOver -= OnLevelOver;
        }
        
        // Private functions
        private void OnLevelStart() {
            StartCoroutine(SpikeRoutine());
        }
        
        private void OnLevelOver() {
            StopAllCoroutines();
        }
        
        private IEnumerator SpikeRoutine() {
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
