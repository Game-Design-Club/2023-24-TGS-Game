using UnityEditor;

using UnityEngine;

namespace Tools.Editor
{
    public static class RotateObject { // Rotate selected object 90 degrees clockwise (helpful for level creation)
        [MenuItem("Edit/Rotate Object 90 Degrees &r")]
        private static void RotateObj() {
            foreach (GameObject obj in Selection.gameObjects) {
                Undo.RecordObject(obj.transform, "Rotate 90 degrees");
                obj.transform.Rotate(0, 0, 90, Space.World);
            }
        }

        // Validate the menu item
        [MenuItem("Edit/Rotate Object 90 Degrees &r", true)]
        private static bool ValidateRotateObj() {
            return Selection.activeTransform != null;
        }
    }
}