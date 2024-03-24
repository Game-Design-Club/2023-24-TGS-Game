using UnityEngine;

namespace Game.NightLevels.Box {
    public class BoxTrigger : MonoBehaviour { // Trigger for the box to attach to other objects
        [SerializeField] public Vector2 AttachDirection; // from perspective of box
    }
}