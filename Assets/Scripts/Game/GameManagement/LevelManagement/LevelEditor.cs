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

            _currentLevel = (Level) target;

            if (GUILayout.Button("Add Object"))
            {
                ObjectSelectionWindow.ShowWindow(); // creates window
            }
        }

        public class ObjectSelectionWindow : EditorWindow
        {
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
                LoadPrefabList();
            }

            private void OnFocus()
            {
                Repaint();
            }

            private void OnGUI()
            {
                CheckDraggedInPrefab();
                
                DisplayPrefabsInList();

                // Handle prefab list reordering by dragging
                
                if (GUILayout.Button("Close"))
                {
                    Close();
                }
            }

            private void DisplayPrefabsInList()
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                
                EditorGUILayout.BeginVertical();
                
                for (int i = 0; i < _objectPrefabList.prefabList.Length; i++)
                {
                    DisplayPrefab(i);
                }
                
                //changing text color
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                Color originalColor = labelStyle.normal.textColor;
                labelStyle.normal.textColor = new Color(originalColor.r * 0.5f, originalColor.g * 0.5f, originalColor.b * 0.5f);
                GUI.skin.label = labelStyle;
                
                //adding info label
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Drag Prefabs to Add");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //reverting color
                labelStyle.normal.textColor = originalColor;
                GUI.skin.label = labelStyle;

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndScrollView();
            }

            private void DisplayPrefab(int i)
            {
                GUILayout.BeginHorizontal(); //start object

                // Display the preview
                Texture2D previewTexture = AssetPreview.GetAssetPreview(_objectPrefabList.prefabList[i]);
                if (previewTexture != null)
                {
                    GUILayout.Label(previewTexture, GUILayout.MaxHeight(50), GUILayout.MaxWidth(50));
                }

                GUILayout.BeginVertical(); //the two lines
                GUILayout.BeginHorizontal(); //starting upper line
                // Display the name of the prefab
                GUILayout.Label(_objectPrefabList.prefabList[i].name, GUILayout.Width(150));
                
                GUILayout.FlexibleSpace(); //pushes to the other end
                
                //deletes object
                if (GUILayout.Button("delete", GUILayout.Width(50)))
                {
                    RemovePrefabFromList(i);
                }
                
                //moves object up in list
                if (i != 0 && GUILayout.Button("Ʌ", GUILayout.Width(20)))
                {
                    SwapPrefabs(i, i - 1);
                }
                GUILayout.EndHorizontal(); //ending upper line

                GUILayout.BeginHorizontal();//starting lower line
                if (GUILayout.Button("Add", GUILayout.Width(60)))
                {
                    AddPrefabToScene(_objectPrefabList.prefabList[i]);
                }
                
                GUILayout.FlexibleSpace(); //pushes to the other side
                
                if (i != _objectPrefabList.prefabList.Length - 1 && GUILayout.Button("V", GUILayout.Width(20)))
                {
                    SwapPrefabs(i, i + 1);
                }
                GUILayout.EndHorizontal(); //ends second layer

                GUILayout.EndVertical(); // End of two layers

                GUILayout.EndHorizontal(); //end of object
            }

            private void AddPrefabToScene(GameObject prefab)
            {
                String parentName = prefab.name + "s";
                GameObject parent = null;

                bool found = false;
                for (int i = 0; i < _currentLevel.transform.childCount; i++)
                {
                    Transform child = _currentLevel.transform.GetChild(i);
                    
                    if (!child.gameObject.name.Equals(parentName)) continue;
                    
                    parent = child.gameObject;
                    found = true;
                    break;
                }

                if (!found)
                {
                    parent = Instantiate(new GameObject(), _currentLevel.transform);
                    parent.name = parentName;
                }
                
                SceneView sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null)
                {
                    Camera sceneCamera = sceneView.camera;
                    Vector3 centerPoint = sceneCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, sceneCamera.nearClipPlane));
                    centerPoint.x = (int)centerPoint.x;
                    centerPoint.y = (int)centerPoint.y;
                    
                    GameObject newObject = Instantiate(prefab, centerPoint, new Quaternion(), parent.transform);
                    newObject.name = prefab.name;
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
                        if (draggedObject is not GameObject) continue;

                        AddPrefabToList((GameObject)draggedObject);
                    }

                    DragAndDrop.AcceptDrag();
                }

                Event.current.Use();
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