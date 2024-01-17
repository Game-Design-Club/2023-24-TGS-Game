using UnityEngine;

namespace Tools.Editor.LevelManipulation
{
    [CreateAssetMenu(fileName = "ObjectPrefabList", menuName = "Object Prefab List", order = 2)]
    public class ObjectPrefabList : ScriptableObject
    {
        public GameObject[] prefabList;
    }
}
