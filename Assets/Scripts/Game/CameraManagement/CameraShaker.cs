using System;
using System.Collections;

using Cinemachine;

using Game.GameManagement;

using UnityEngine;

namespace Game.CameraManagement {
    public class CameraShaker : MonoBehaviour {
        [SerializeField] private float cutoff = 0.01f;
        [SerializeField] private CameraShakeData defaults;
        
        private CinemachineVirtualCamera _cmvc;
        private CinemachineBasicMultiChannelPerlin _cbmcp;

        public static CameraShaker Instance;
        
        // Unity functions
        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                Debug.LogWarning("Duplicate CameraShaker found and deleted.");
            } else {
                Instance = this;
            }
            _cmvc = GetComponent<CinemachineVirtualCamera>();
            _cbmcp = _cmvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable() {
            GameManager.Instance.OnLevelStart += OnLevelStart;
        }
        
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
        }

        private void Start() {
            _cbmcp.m_AmplitudeGain = 0;
            _cbmcp.m_FrequencyGain = 0;
        }

        // Private functions
        private void InternalShake(CameraShakeData data) {
            StartCoroutine(ShakeCoroutine(data));
        }
        private IEnumerator ShakeCoroutine(CameraShakeData data) {
            _cbmcp.m_AmplitudeGain += data.intensity;
            _cbmcp.m_FrequencyGain += data.frequency;
            if (data.decay) {
                float timer = data.time;
                while (timer > 0) {
                    _cbmcp.m_AmplitudeGain -= data.intensity * Time.deltaTime / data.time;
                    _cbmcp.m_FrequencyGain -= data.frequency * Time.deltaTime / data.time;
                    
                    timer -= Time.deltaTime;
                    yield return null;
                }
            } else {
                yield return new WaitForSeconds(data.time);
                _cbmcp.m_AmplitudeGain -= data.intensity;
                _cbmcp.m_FrequencyGain -= data.frequency;
            }
            
            if (_cbmcp.m_AmplitudeGain < cutoff) _cbmcp.m_AmplitudeGain = 0;
            if (_cbmcp.m_FrequencyGain < cutoff) _cbmcp.m_FrequencyGain = 0;
        }

        private void OnLevelStart() {
            StopAllCoroutines();
            _cbmcp.m_AmplitudeGain = 0;
            _cbmcp.m_FrequencyGain = 0;
            
        }
        
        // Public functions
        public void Shake(CameraShakeData data) {
            Instance.InternalShake(data);
        }
        public void Shake(float intensity, float frequency, float time, bool decay) {
            Shake(new CameraShakeData(intensity, frequency, time, decay));
        }
        public void Shake() {
            Shake(defaults);
        }
    }
}