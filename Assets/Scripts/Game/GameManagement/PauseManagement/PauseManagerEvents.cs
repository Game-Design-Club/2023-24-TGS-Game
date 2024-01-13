using System;

namespace Game.GameManagement.PauseManagement {
    public static class PauseManagerEvents {
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