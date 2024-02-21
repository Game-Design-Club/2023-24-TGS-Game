using Game.PlayerComponents;

using Tools.Constants;

using UnityEngine;

namespace Game.NightLevels.Box {
    public class Box : MonoBehaviour {
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider;
        
        // Unity functions
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }
        
        // Public functions
        public RaycastHit2D SendBoxCast(Vector2 direction, float distance, LayerMask wallLayer) {
            Vector2 size = _boxCollider.size * transform.localScale;
            RaycastHit2D hit = Physics2D.BoxCast(_rigidbody2D.position, size, 0f, direction, Mathf.Abs(distance), wallLayer);
            
            RaycastHit2D[] hits = new RaycastHit2D[4];
            Physics2D.BoxCastNonAlloc(_rigidbody2D.position, size, 0f, direction, hits, Mathf.Abs(distance), wallLayer);
            
            PlayerBoxMover.BoxChain.Add(_rigidbody2D);
            
            foreach (RaycastHit2D currentHit in hits) {
                if (currentHit.collider == null) return currentHit;
                if (currentHit.collider.gameObject.CompareTag(TagConstants.Box)) {
                    RaycastHit2D chainHit = currentHit.collider.GetComponent<Box>().SendBoxCast(direction, distance, wallLayer);

                    hit = chainHit;
                } else {
                    Debug.Log("Hit wall");
                    return currentHit;
                }
            }
            
            return hit;
        }
    }
}