using UnityEngine;

namespace Game.NightLevels.Shooter {
    public class BulletHolder : MonoBehaviour {
        public static Transform BulletHolderTransform;
        
        private void Awake() {
            BulletHolderTransform = transform;
        }
    }
}