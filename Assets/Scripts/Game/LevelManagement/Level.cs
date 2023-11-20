using UnityEngine;
using UnityEngine.Serialization;

namespace Game.LevelManagement {
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
    public class Level : ScriptableObject {
        [SerializeField] public string levelName = "Unnamed Level";
        [SerializeField] public GameObject gameObject;
        [SerializeField] public Level nextLevel;
        // [SerializeField] public Music music;

        public override string ToString() {
            return levelName;
        }
    }
}