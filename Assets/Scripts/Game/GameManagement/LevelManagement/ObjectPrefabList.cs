using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameManagement.LevelManagement
{
    [CreateAssetMenu(fileName = "ObjectPrefabList", menuName = "Object Prefab List", order = 2)]
    public class ObjectPrefabList : ScriptableObject
    {
        public GameObject[] prefabList;
    }
}
