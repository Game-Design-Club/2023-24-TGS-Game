using System;

namespace Game.GameManagement {
    public static class GameManagerEvents {
        public static event Action OnLevelStart; // LevelStart is called when the level is loaded
        public static event Action OnLevelOver; // LevelOver is called when the player dies or finishes the level
        
        internal static void InvokeLevelStart() {
            OnLevelStart?.Invoke();
        }
        
        internal static void InvokeLevelOver() {
            OnLevelOver?.Invoke();
        }
    }
}