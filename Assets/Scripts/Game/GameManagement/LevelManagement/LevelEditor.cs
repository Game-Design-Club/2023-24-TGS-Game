using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
            private GameObject[] _prefabList;
            private Vector2 _scrollPosition;

            private int _selectedPrefabIndex = 0;
            private Rect[] _prefabRects;

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
                _prefabRects = new Rect[_prefabList.Length];
            }

            private void OnGUI()
            {
                // Add your GUI elements for the custom window here
                // GUILayout.Label("Level Objects:");

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
                for (int i = 0; i < _prefabList.Length; i++)
                {
                    GUILayout.BeginHorizontal();

                    // Display the preview
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(_prefabList[i]);
                    if (previewTexture != null)
                    {
                        GUILayout.Label(previewTexture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50));
                    }

                    GUILayout.BeginVertical(); // Nested vertical layout for name and button

                    // Display the name of the prefab
                    GUILayout.Label(_prefabList[i].name, GUILayout.Width(100));

                    // Add a button under the name
                    if (GUILayout.Button("Add", GUILayout.Width(60)))
                    {
                        GameObject newObject = Instantiate(_prefabList[i], _currentLevel.transform);
                        newObject.name = _prefabList[i].name;
                    }

                    GUILayout.EndVertical(); // End of nested vertical layout

                    GUILayout.EndHorizontal();
                    
                    // Assign the reordering rectangle
                    _prefabRects[i] = GUILayoutUtility.GetLastRect();
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();

                // Handle prefab list reordering by dragging
                HandlePrefabReordering();
                
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
            }

            private void HandlePrefabReordering()
            {
                Event e = Event.current;
                switch (e.type)
                {
                    case EventType.MouseDown:
                        for (int i = 0; i < _prefabRects.Length; i++)
                        {
                            if (_prefabRects[i].Contains(e.mousePosition))
                            {
                                _selectedPrefabIndex = i;
                                break;
                            }
                        }

                        break;

                    case EventType.MouseDrag:
                        if (e.button == 0 && _prefabRects[_selectedPrefabIndex].Contains(e.mousePosition))
                        {
                            DragAndDrop.PrepareStartDrag();
                            DragAndDrop.SetGenericData("PrefabDrag", _prefabList[_selectedPrefabIndex]);
                            DragAndDrop.StartDrag(_prefabList[_selectedPrefabIndex].name);
                            e.Use();
                        }

                        break;

                    case EventType.MouseUp:
                        if (DragAndDrop.GetGenericData("PrefabDrag") != null)
                        {
                            GameObject draggedPrefab = DragAndDrop.GetGenericData("PrefabDrag") as GameObject;

                            if (draggedPrefab != null)
                            {
                                int draggedIndex = System.Array.IndexOf(_prefabList, draggedPrefab);
                                int targetIndex = -1;

                                for (int i = 0; i < _prefabRects.Length; i++)
                                {
                                    if (_prefabRects[i].Contains(e.mousePosition))
                                    {
                                        targetIndex = i;
                                        break;
                                    }
                                }

                                if (targetIndex != -1 && draggedIndex != targetIndex)
                                {
                                    // Reorder the prefab list
                                    (_prefabList[draggedIndex], _prefabList[targetIndex]) = (_prefabList[targetIndex], _prefabList[draggedIndex]);

                                    _selectedPrefabIndex = targetIndex;
                                }
                                DragAndDrop.SetGenericData("PrefabDrag", null);
                                SavePrefabListToFile();
                            }
                        }

                        break;
                    case EventType.Repaint:
                        if (DragAndDrop.GetGenericData("PrefabDrag") != null)
                        {
                            GUIStyle draggedStyle = new GUIStyle(GUI.skin.box);
                            draggedStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/moveareatarget.png") as Texture2D;

                            Rect r = _prefabRects[_selectedPrefabIndex];
                            GUI.Box(r, GUIContent.none, draggedStyle);
                        }
                        break;
                }
            }
            
            private void LoadPrefabList()
            {
                string filePath = "Assets/Scripts/Game/GameManagement/LevelManagement/SavedPrefabs.txt";

                if (File.Exists(filePath))
                {
                    string[] savedPrefabPaths = File.ReadAllLines(filePath);

                    // Initialize the prefab list
                    _prefabList = new GameObject[savedPrefabPaths.Length];

                    for (int i = 0; i < savedPrefabPaths.Length; i++)
                    {
                        _prefabList[i] = AssetDatabase.LoadAssetAtPath<GameObject>(savedPrefabPaths[i]);
                    }
                }
                else
                {
                    Debug.LogError("SavedPrefabs.txt not found. Please create the file and add prefab paths.");
                    _prefabList = new GameObject[0]; // Initialize an empty list
                }
            }

            private void AddPrefabToList(GameObject prefab)
            {
                // Check if the prefab is already in the list
                if (System.Array.Exists(_prefabList, element => element == prefab))
                {
                    Debug.LogWarning("Prefab is already in the list.");
                    return;
                }

                // Resize the prefab list array
                System.Array.Resize(ref _prefabList, _prefabList.Length + 1);
                System.Array.Resize(ref _prefabRects, _prefabRects.Length + 1);

                // Add the new prefab to the list
                _prefabList[_prefabList.Length - 1] = prefab;

                SavePrefabListToFile();

                // Repaint the window to reflect the changes
                Repaint();
            }

            private void SavePrefabListToFile()
            {
                string filePath = "Assets/Scripts/Game/GameManagement/LevelManagement/SavedPrefabs.txt";

                // Create or overwrite the text file
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (GameObject prefab in _prefabList)
                    {
                        string prefabPath = AssetDatabase.GetAssetPath(prefab);
                        writer.WriteLine(prefabPath);
                    }
                }

            }
        }
    }
}