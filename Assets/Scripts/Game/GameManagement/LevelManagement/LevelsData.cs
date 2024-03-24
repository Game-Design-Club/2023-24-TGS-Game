using UnityEngine;

namespace Game.GameManagement.LevelManagement {
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Game/LevelsData")]
    public class LevelsData : ScriptableObject { // Stores all the levels data
        [SerializeField] public Level[] levels;
        
        // Array access operator
        public Level this[int index] {
            get {
                if (levels.Length == 0) {
                    Debug.LogWarning("Tried to access levels data with no levels");
                }
                if (index < 0 || index >= levels.Length) {
                    return null;
                }
                return levels[index];
            }
            set => levels[index] = value;
        }
    }
}