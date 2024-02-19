using Tools.Constants;

using UnityEngine;

namespace Game.NightLevels.Box {
    public class Box : MonoBehaviour {
        private Rigidbody2D _rigidbody2D;
        
        // Unity functions
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        // Public functions
        public RaycastHit2D SendBoxCast(Vector2 direction, float distance, LayerMask wallLayer) {
            Vector2 size = transform.localScale;
            RaycastHit2D hit = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, direction, Mathf.Abs(distance), wallLayer);
            
            if (hit.collider != null && hit.collider.gameObject.CompareTag(TagConstants.Box) && hit.collider.gameObject != gameObject) {
                // hit = hit.collider.GetComponent<Box>().SendBoxCast(direction, distance, layerMask);
                Debug.Log("Double box :O " + (hit.collider.gameObject == gameObject));
            }
            Debug.Log(distance + "     " + hit.distance);
            return hit;
        }
    }
}