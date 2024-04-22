using System.Collections;

using AppCore.AudioManagement;

using UnityEngine;
using UnityEngine.Rendering.Universal;

using Tools;

namespace Game.Ambience {
    public class FlickeringLights : MonoBehaviour {
        [SerializeField] private Light2D[] lights;

        [SerializeField] private IntRange batchSize = new (1, 5);
        [SerializeField] private FloatRange flickerIntensity = new(.2f, .5f);
        [SerializeField] private FloatRange flickerSustainTime = new(.1f, .2f);
        [SerializeField] private FloatRange flickerSeparateTime = new(.1f, .2f);
        
        [SerializeField] private FloatRange batchSeparateTime = new (5, 10);

        [SerializeField] private SoundPackage flickerSound;
        
        // Unity functions
        private void Awake() {
            if (lights.Length == 0) {
                lights = GetComponents<Light2D>();
                if (lights.Length == 0) {
                    Debug.LogWarning("No light found for flickering ambience", this);
                }
            }
        }

        private void Start() {
            StartCoroutine(FlickerLights());
        }
        
        // Private functions
        private IEnumerator FlickerLights() {
            yield return new WaitForSeconds(Random.Range(0, batchSeparateTime.max));

            while (true) {
                // New Batch
                for (int i = 0; i < batchSize.Random(); i++) {
                    // Single Flicker
                    float currentIntensityChange = flickerIntensity.Random();
                    ChangeLightIntensity(1 + currentIntensityChange);
                    yield return new WaitForSeconds(flickerSustainTime.Random());
                    ChangeLightIntensity(1/(1 + currentIntensityChange));
                    yield return new WaitForSeconds(flickerSeparateTime.Random());
                }
                yield return new WaitForSeconds(batchSeparateTime.Random());
            }

            void ChangeLightIntensity(float amount) {
                foreach (Light2D light in lights) {
                    light.intensity /= amount;
                }
            }
        }
    }
}