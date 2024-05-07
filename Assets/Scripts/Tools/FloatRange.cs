using System;

using UnityEngine;

namespace Tools {
    [Serializable]
    public struct FloatRange {
        [SerializeField] public float max;
        [SerializeField] public float min;

        public FloatRange(float max, float min) {
            this.max = max;
            this.min = min;
        }
        
        public float Random() {
            return UnityEngine.Random.Range(min, max);
        }
    }
}