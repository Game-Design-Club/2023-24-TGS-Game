using System;

using Constants;

using UnityEngine;

namespace Tools.ScriptTools {
    public static class CollisionExtension {
        private static String[] blockingTags = {LayerConstants.Walls, LayerConstants.Blocking};
        public static bool IsTouchingBlocking(this Collision2D collision) {
            foreach (String tag in blockingTags) {
                if (collision.gameObject.CompareTag(tag)) {
                    return true;
                }
            }
            return false;
        }
    }
}