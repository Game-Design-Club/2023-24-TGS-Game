using System;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.GameManagement.LevelManagement {
    public class Level : MonoBehaviour { // Marks as a level, used to store level data
        [SerializeField] public string levelName = "Unnamed Level";
        public override string ToString() {
            return levelName;
        }

        private void Awake() {
            Light2D light = GetComponent<Light2D>();
            if (light != null) {
                if (light.lightType == Light2D.LightType.Global) {
                    Destroy(light);
                }
            }
        }
    }
}