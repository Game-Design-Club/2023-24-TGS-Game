using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    public class Level : MonoBehaviour { // Marks as a level, used to store level data
        [SerializeField] public string levelName = "Unnamed Level";
        public override string ToString() {
            return levelName;
        }
    }
}