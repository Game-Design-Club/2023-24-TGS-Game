using System;
using System.Collections.Generic;
using System.Linq;

using Game.GameManagement.LevelManagement;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Tools.Editor.LevelManipulation
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor { // Custom editor for the Level script
        private static Level s_currentLevel;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // This will draw the default Inspector fields for the script

            s_currentLevel = (Level) target;

            if (GUILayout.Button("Add Object"))
            {
                ObjectSelectionWindow.ShowWindow(); // creates window
            }
        }

        public class ObjectSelectionWindow : EditorWindow
        {
            private ObjectPrefabList _objectPrefabList;
            private Vector2 _scrollPosition;
            private String _searchText = "";
            private bool _searchBarFocused;
            
            [MenuItem("Window/Custom Popup Window")]
            public static void ShowWindow()
            {
                // Get existing open window or if none, create a new one
                ObjectSelectionWindow window = GetWindow<ObjectSelectionWindow>("Level Objects");
                window.minSize = new Vector2(300, 200);
            }

            private void OnEnable()
            {
                LoadPrefabList();
            }

            private void OnFocus()
            {
                Repaint();
            }

            private void OnGUI()
            {
                CheckDraggedInPrefab();
                
                SearchBar();
                
                DisplayPrefabsInList();

                // Handle prefab list reordering by dragging
                
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
            }

            private void SearchBar()
            {
                using var searchbarArea = new EditorGUILayout.HorizontalScope();
                
                EditorGUI.BeginChangeCheck();
                
                // Detect when the user enters the TextField
                GUI.SetNextControlName("Search Bar"); // Set a control name
                
                //changing text color
                Color originalColor = GUI.contentColor;
                GUI.contentColor = _searchBarFocused || !_searchText.Equals("") ? originalColor : new Color(originalColor.r * 0.75f, originalColor.g * 0.75f, originalColor.b * 0.75f);
                
                String currentText = GUILayout.TextField(_searchBarFocused || !_searchText.Equals("") ? _searchText : "Search");
                
                //reverting color
                GUI.contentColor = originalColor;
                
                if (_searchBarFocused)
                {
                    _searchText = currentText;
                }

                if (Event.current.type == EventType.Repaint && GUI.GetNameOfFocusedControl() == "Search Bar")
                {
                    // TextField is focused
                    if (!_searchBarFocused)
                    {
                        _searchBarFocused = true;
                    }
                    
                }
                
                if (Event.current.type == EventType.MouseDown && _searchBarFocused)
                {
                    Rect textFieldRect = GUILayoutUtility.GetLastRect();

                    // Check if the click is outside the text field
                    if (!textFieldRect.Contains(Event.current.mousePosition))
                    {
                        // Lose focus when clicking outside the text field
                        EditorGUI.FocusTextInControl(null);
                        _searchBarFocused = false;
                        Repaint(); // Force repaint to update the GUI
                    }
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    SetFocusToSearchBar();
                }
            }

            private void SetFocusToSearchBar()
            {
                GUI.FocusControl("Search Bar");
                _searchText = "";
                _searchBarFocused = true;
            }

            private void DisplayPrefabsInList()
            {
                using var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition);
                _scrollPosition = scrollView.scrollPosition;

                using var wholeArea = new EditorGUILayout.VerticalScope();
                
                for (int i = 0; i < _objectPrefabList.prefabList.Length; i++)
                {
                    if (!_objectPrefabList.prefabList[i].name.ToLower().Contains(_searchText.ToLower()))
                        continue;
                    DisplayPrefab(i);
                }
                

                //changing text color
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                Color originalColor = labelStyle.normal.textColor;
                labelStyle.normal.textColor = new Color(originalColor.r * 0.5f, originalColor.g * 0.5f,
                    originalColor.b * 0.5f);
                GUI.skin.label = labelStyle;

                //adding info label
                using (var infoLabel = new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Drag Prefabs to Add");
                    GUILayout.FlexibleSpace();
                }

                //reverting color
                labelStyle.normal.textColor = originalColor;
                GUI.skin.label = labelStyle;
            }

            private void DisplayPrefab(int i)
            {
                using var prefabArea = new EditorGUILayout.HorizontalScope();
                
                // Display the preview
                Texture2D previewTexture = AssetPreview.GetAssetPreview(_objectPrefabList.prefabList[i]);
                if (previewTexture != null)
                {
                    GUILayout.Label(previewTexture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50));
                }

                using var actionsArea = new EditorGUILayout.VerticalScope();
                
                using (var upperArea = new EditorGUILayout.HorizontalScope())
                {
                    // Display the name of the prefab
                    GUILayout.Label(_objectPrefabList.prefabList[i].name, GUILayout.Width(150));

                    GUILayout.FlexibleSpace(); //pushes to the other end

                    //deletes object
                    if (GUILayout.Button("delete", GUILayout.Width(50)))
                    {
                        RemovePrefabFromList(i);
                    }

                    //moves object up in list
                    if (i == 0 && _searchText.Equals(""))
                    {
                        EditorGUILayout.Space(20, false);
                    }
                    else if (_searchText.Equals("") && GUILayout.Button("Ʌ", GUILayout.Width(20)))
                    {
                        SwapPrefabs(i, i - 1);
                    }

                }

                using (var lowerArea = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add", GUILayout.Width(60)))
                    {
                        AddPrefabToScene(_objectPrefabList.prefabList[i]);
                    }

                    GUILayout.FlexibleSpace(); //pushes to the other side

                    if (i != _objectPrefabList.prefabList.Length - 1 && _searchText.Equals("") &&
                        GUILayout.Button("V", GUILayout.Width(20)))
                    {
                        SwapPrefabs(i, i + 1);
                    }
                }
            }

            private void AddPrefabToScene(GameObject prefab)
            {
                String parentName = prefab.name + "s";
                GameObject parent = null;

                bool found = false;
                for (int i = 0; i < s_currentLevel.transform.childCount; i++)
                {
                    Transform child = s_currentLevel.transform.GetChild(i);
                    
                    if (!child.gameObject.name.Equals(parentName)) continue;
                    
                    parent = child.gameObject;
                    found = true;
                    break;
                }

                if (!found)
                {
                    parent = Instantiate(new GameObject(), s_currentLevel.transform);
                    parent.name = parentName;
                }
                
                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null)
                {
                    Camera sceneCamera = sceneView.camera;
                    Vector3 centerPoint = sceneCamera.ViewportToWorldPoint(new Vector3(sceneCamera.rect.center.x, sceneCamera.rect.center.y, 0));
                    centerPoint.z = 0;
                    
                    GameObject newObject = PrefabUtility.InstantiatePrefab(prefab, parent.transform) as GameObject;
                    newObject.transform.position = centerPoint;
                    newObject.name = prefab.name;
                    
                    Undo.RegisterCreatedObjectUndo(newObject, "Add " + prefab.name);
                }
                else
                {
                    Debug.LogError("No active Scene View found.");
                }
            }

            private void SwapPrefabs(int index1, int index2)
            {
                (_objectPrefabList.prefabList[index1], _objectPrefabList.prefabList[index2]) = (
                    _objectPrefabList.prefabList[index2], _objectPrefabList.prefabList[index1]);
            }
            
            private void CheckDraggedInPrefab()
            {
                Event e = Event.current;
                // Check if there's a prefab dragged into the window
                if (e.type != EventType.DragUpdated && e.type != EventType.DragPerform) return;
                
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (e.type == EventType.DragPerform)
                {
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (!(draggedObject is GameObject)) continue;

                        AddPrefabToList((GameObject) draggedObject);
                    }

                    DragAndDrop.AcceptDrag();
                }

                Event.current.Use();
            }

            private void LoadPrefabList()
            {
                string scriptableObjectPath = "Assets/Scripts/Tools/Editor/LevelManipulation/ObjectPrefabList.asset";

                _objectPrefabList = AssetDatabase.LoadAssetAtPath<ObjectPrefabList>(scriptableObjectPath);
                if (_objectPrefabList == null) {
                    Debug.LogError("ObjectPrefabList not found. Please create one at " + scriptableObjectPath + " and add it to the LevelEditor window.");
                }
            }

            private void AddPrefabToList(GameObject prefab)
            {
                // Check if the prefab is already in the list
                if (Array.Exists(_objectPrefabList.prefabList, element => element == prefab))
                {
                    Debug.LogWarning("Prefab is already in the list.");
                    return;
                }

                // Resize the prefab list array
                Array.Resize(ref _objectPrefabList.prefabList, _objectPrefabList.prefabList.Length + 1);

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