using System;

namespace Game.GameManagement.PauseManagement {
    public static class PauseManagerEvents { // Events for the PauseManager for other classes to subscribe to
        public static event Action OnGamePause;
        public static event Action OnGameResume;
        
        internal static void InvokeGamePause() {
            OnGamePause?.Invoke();
        }
        
        internal static void InvokeGameResume() {
            OnGameResume?.Invoke();
        }
    }
}