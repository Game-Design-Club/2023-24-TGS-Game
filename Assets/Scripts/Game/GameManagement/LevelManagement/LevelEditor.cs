using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Game.GameManagement.LevelManagement
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private static Level _currentLevel;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // This will draw the default Inspector fields for the script

            _currentLevel = (Level)target;

            if (GUILayout.Button("Add Object"))
            {
                ObjectSelectionWindow.ShowWindow();
            }
        }

        public class ObjectSelectionWindow : EditorWindow
        {
            // Add any properties or methods for your custom window here
            private ObjectPrefabList _objectPrefabList;
            private Vector2 _scrollPosition;
            
            [MenuItem("Window/Custom Popup Window")]
            public static void ShowWindow()
            {
                // Get existing open window or if none, create a new one
                ObjectSelectionWindow window = GetWindow<ObjectSelectionWindow>("Level Objects");
                window.minSize = new Vector2(300, 200);
            }

            private void OnEnable()
            {
                // Initialize your prefab list here
                LoadPrefabList();
            }

            private void OnFocus()
            {
                Repaint();
            }

            private void OnGUI()
            {
                // Add your GUI elements for the custom window here
                GUILayout.Label("Level Objects:");

                Event e = Event.current;

                // Check if there's a prefab dragged into the window
                if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (e.type == EventType.DragPerform)
                    {
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is GameObject)
                            {
                                // Add the dragged prefab to the list
                                AddPrefabToList((GameObject)draggedObject);
                            }
                        }

                        // Accept the drag event
                        DragAndDrop.AcceptDrag();
                    }

                    Event.current.Use();
                }

                // Display a preview, name, and button for each prefab in the list
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                
                EditorGUILayout.BeginVertical();
                
                for (int i = 0; i < _objectPrefabList.prefabList.Length; i++)
                {
                    GUILayout.BeginHorizontal();

                    // Display the preview
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(_objectPrefabList.prefabList[i]);
                    if (previewTexture != null)
                    {
                        GUILayout.Label(previewTexture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50));
                    }

                    GUILayout.BeginVertical(); // Nested vertical layout for name and button
                    GUILayout.BeginHorizontal();
                    // Display the name of the prefab
                    GUILayout.Label(_objectPrefabList.prefabList[i].name, GUILayout.Width(150));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("delete", GUILayout.Width(50)))
                    {
                        RemovePrefabFromList(i);
                    }
                    if (i != 0 && GUILayout.Button("Ʌ", GUILayout.Width(20)))
                    {
                        (_objectPrefabList.prefabList[i - 1], _objectPrefabList.prefabList[i]) = (_objectPrefabList.prefabList[i], _objectPrefabList.prefabList[i - 1]);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    // Add a button under the name
                    if (GUILayout.Button("Add", GUILayout.Width(60)))
                    {
                        GameObject newObject = Instantiate(_objectPrefabList.prefabList[i], _currentLevel.transform);
                        newObject.name = _objectPrefabList.prefabList[i].name;
                    }
                    GUILayout.FlexibleSpace();
                    if (i != _objectPrefabList.prefabList.Length - 1 && GUILayout.Button("V", GUILayout.Width(20)))
                    {
                        (_objectPrefabList.prefabList[i + 1], _objectPrefabList.prefabList[i]) = (_objectPrefabList.prefabList[i], _objectPrefabList.prefabList[i + 1]);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical(); // End of nested vertical layout

                    GUILayout.EndHorizontal();
                    
                    // Assign the reordering rectangle
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();

                // Handle prefab list reordering by dragging
                
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
            }

            private void LoadPrefabList()
            {
                string scriptableObjectPath = "Assets/Scripts/Game/GameManagement/LevelManagement/ObjectPrefabList.asset";

                _objectPrefabList = AssetDatabase.LoadAssetAtPath<ObjectPrefabList>(scriptableObjectPath);

            }

            private void AddPrefabToList(GameObject prefab)
            {
                // Check if the prefab is already in the list
                if (System.Array.Exists(_objectPrefabList.prefabList, element => element == prefab))
                {
                    Debug.LogWarning("Prefab is already in the list.");
                    return;
                }

                // Resize the prefab list array
                System.Array.Resize(ref _objectPrefabList.prefabList, _objectPrefabList.prefabList.Length + 1);

                // Add the new prefab to the list
                _objectPrefabList.prefabList[^1] = prefab;

                // Repaint the window to reflect the changes
                Repaint();
            }
            
            private void RemovePrefabFromList(int index)
            {
                List<GameObject> list = _objectPrefabList.prefabList.ToList();
                list.RemoveAt(index);
                _objectPrefabList.prefabList = list.ToArray();

                // Repaint the window to reflect the changes
                Repaint();
            }

        }
    }
}