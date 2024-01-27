using UnityEditor;

using UnityEngine;

namespace Tools.Editor
{
    public static class RotateObject {
        [MenuItem("Edit/Rotate Object 90 Degrees &_r")] // %r is the shortcut for Ctrl+R or Command+R
        private static void RotateObj() {
            foreach (GameObject obj in Selection.gameObjects) {
                Undo.RecordObject(obj.transform, "Rotate 90 degrees");
                obj.transform.Rotate(0, 0, 90, Space.World);
            }
        }

        // Validate the menu item
        [MenuItem("Edit/Rotate Object 90 Degrees _%r", true)]
        private static bool ValidateRotateObj() {
            // Return false if no transform is selected
            return Selection.activeTransform != null;
        }
    }
}