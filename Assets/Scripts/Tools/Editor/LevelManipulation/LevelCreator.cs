using System.IO;

using UnityEditor;

using UnityEngine;

namespace Tools.Editor.LevelManipulation
{
    public class LevelCreator
    {
        private const string DefaultGameObjectPath = "Assets/Levels/Level Template/Level Template.prefab"; // Path to the default GameObject

        [MenuItem("Tools/Create New Level")]
        private static void CreateLevelPrefabFromDefault()
        {
            GameObject defaultGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(DefaultGameObjectPath);
            if (defaultGameObject == null)
            {
                EditorUtility.DisplayDialog("Default GameObject Missing", "The default GameObject could not be found at the specified path.", "OK");
                return;
            }

            // Get the currently open folder in Unity
            string folderPath = GetCurrentFolderPath();

            // Instantiate and create a prefab from the default GameObject
            GameObject instantiatedObject = PrefabUtility.InstantiatePrefab(defaultGameObject) as GameObject;
            if (instantiatedObject == null) {
                EditorUtility.DisplayDialog("Prefab Creation Failed", "Failed to create a prefab.", "OK");
                return;
            }
            PrefabUtility.UnpackPrefabInstance(instantiatedObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            string prefabName = "New Level";
            instantiatedObject.name = prefabName;
            string fullPath = Path.Combine(folderPath, prefabName + ".prefab");

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instantiatedObject, fullPath);
            Object.DestroyImmediate(instantiatedObject);

            if (prefab == null)
            {
                EditorUtility.DisplayDialog("Prefab Creation Failed", "Failed to create a prefab.", "OK");
                return;
            }

            // Prompt for renaming
            AssetDatabase.OpenAsset(prefab);
            EditorGUIUtility.PingObject(prefab);
        }

        private static string GetCurrentFolderPath()
        {
            // Default to the Assets directory
            string path = "Assets/Levels";

            // Attempt to use the path of the last selected object in the Project window
            if (Selection.activeObject != null)
            {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (path.Contains("."))
                {
                    // If it's a file, get the directory
                    path = Path.GetDirectoryName(path);
                }
            }

            return path;
        }
    }
}
