using System;

using UnityEngine;

namespace Tools {
    [Serializable]
    public struct IntRange {
        [SerializeField] public int min;
        [SerializeField] public int max;

        public IntRange(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public int Random() {
            return UnityEngine.Random.Range(min, max);
        }
    }
}