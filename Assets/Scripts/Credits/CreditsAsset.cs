using UnityEngine;

namespace Credits {
    [CreateAssetMenu(fileName = "Credits Asset", menuName = "Credits Asset", order = 0)]
    public class CreditsAsset : ScriptableObject { // Stores the credits data to be passed to the CreditsManager
        [SerializeField] public CreditsSection[] creditsSections;
        [SerializeField] public float spaceBetweenSections;
    }
}