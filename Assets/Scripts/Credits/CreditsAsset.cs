using UnityEngine;

namespace Credits {
    [CreateAssetMenu(fileName = "Credits Asset", menuName = "Credits Asset", order = 0)]
    public class CreditsAsset : ScriptableObject {
        [SerializeField] public CreditsSection[] creditsSections;
        [SerializeField] public float spaceBetweenSections;
    }
}