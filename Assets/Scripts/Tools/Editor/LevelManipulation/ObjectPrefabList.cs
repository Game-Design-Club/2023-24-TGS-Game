using UnityEngine;

namespace Tools.Editor.LevelManipulation
{
    [CreateAssetMenu(fileName = "ObjectPrefabList", menuName = "Object Prefab List", order = 2)]
    public class ObjectPrefabList : ScriptableObject { // List of prefabs to be used in the level creator, saved to a file
        public GameObject[] prefabList;
    }
}
