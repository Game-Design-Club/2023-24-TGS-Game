namespace Tools.Constants {
    public static class AnimationConstants { // Animation constants to ensure spelling consistency
        public static class FadeInOut {
            public const string FadeToBlack = "FadeToBlack";
            public const string FadeFromBlack = "FadeFromBlack";
        }
        
        public static class WipeInOut {
            public const string WipeToBlack = "WipeToBlack";
            public const string WipeFromBlack = "WipeFromBlack";
        }
        
        public static class InteractionsPopup {
            public const string Show = "Show";
            public const string Hide = "Hide";
        }

        public static class Player {
            public const string Die = "Die";
            public const string Direction = "Direction";
            public const int Right = 0;
            public const int Down = 1;
            public const int Left = 3;
            public const int Up = 2;
        }

        public static class Box {
            public const string Grab = "Grab";
            public const string InRange = "Hover";
        }
        
        public static class Interactable {
            public const string Hover = "Hover";
            public const string Interact = "Interact";
        }
        
        public static class MainMenu {
            public const string ShowOptions = "Show Options";
            public const string HideOptions = "Show Main";
            public const string ShowConfirm = "Show Confirm";
            public const string HideConfirm = "Hide Confirm";
        }

        public static class SecurityCamera {
            public const string LockedOnPlayer = "Locked On Player";
        }
    }
}