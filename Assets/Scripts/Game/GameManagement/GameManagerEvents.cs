using System;

namespace Game.GameManagement {
    public static class GameManagerEvents { // Events for the GameManager for other classes to subscribe to
        public static event Action OnLevelStart; // LevelStart is called when the level is loaded
        public static event Action OnLevelOver; // LevelOver is called when the player dies or finishes the level
        public static event Action OnNextLevel; // NextLevel is called when a level is completed
        
        internal static void InvokeLevelStart() {
            OnLevelStart?.Invoke();
        }
        
        internal static void InvokeLevelOver() {
            OnLevelOver?.Invoke();
        }
        
        internal static void InvokeNextLevel() {
            OnNextLevel?.Invoke();
        }
    }
}