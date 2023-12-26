using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    public class Level : MonoBehaviour {
        [SerializeField] public string levelName = "Unnamed Level";
        [SerializeField] public Level nextLevel;
        public override string ToString() {
            return levelName;
        }
    }
}