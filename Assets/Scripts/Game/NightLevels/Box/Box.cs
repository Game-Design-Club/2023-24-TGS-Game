using Game.PlayerComponents;

using Tools.Constants;

using UnityEngine;

namespace Game.NightLevels.Box {
    public class Box : MonoBehaviour { // Box class for the box object
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider;
        private Animator _animator;
        
        // Unity functions
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }
        
        // Public functions
        public RaycastHit2D SendBoxCast(Vector2 direction, float distance, LayerMask wallLayer) {
            // Box cast is a raycast, not the actual box lol
            
            Vector2 size = _boxCollider.size * transform.localScale;
            
            RaycastHit2D[] hits = new RaycastHit2D[4];
            RaycastHit2D hit = hits[0];
            Physics2D.BoxCastNonAlloc(_rigidbody2D.position, size, 0f, direction, hits, Mathf.Abs(distance), wallLayer);
            
            PlayerBoxMover.BoxChain.Add(_rigidbody2D);
            
            foreach (RaycastHit2D currentHit in hits) {
                if (currentHit.collider == null) continue;
                if (currentHit.collider.gameObject.CompareTag(TagConstants.Box)) {
                    // We hit another box, so we need to check if that box hits a wall, and so on
                    RaycastHit2D chainHit = currentHit.collider.GetComponent<Box>().SendBoxCast(direction, distance, wallLayer);

                    hit = chainHit;
                } else {
                    return currentHit;
                }
            }
            
            return hit;
        }

        public void EnteredTrigger() {
            _animator.SetBool(AnimationConstants.Box.InRange, true);
        }

        public void ExitedTrigger() {
            _animator.SetBool(AnimationConstants.Box.InRange, false);
        }

        public void GrabbedBox() {
            _animator.SetBool(AnimationConstants.Box.Grab, true);
        }

        public void ReleasedBox() {
            _animator.SetBool(AnimationConstants.Box.Grab, false);
        }
    }
}