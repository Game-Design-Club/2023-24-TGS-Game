using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Interactables
{
    public class Door : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float closedOpacity = .3f;
        [SerializeField] private ShadowCaster2D shadowCaster2D;
        [SerializeField] private Collider2D objectCollider;
        [SerializeField] private Light2D light2D;
        
        public void Opened() {
            Color c = spriteRenderer.color;
            spriteRenderer.color = new Color(c.r, c.g, c.b, closedOpacity);
            shadowCaster2D.enabled = false;
            objectCollider.enabled = false;
            light2D.enabled = false;
        }
    }
}
