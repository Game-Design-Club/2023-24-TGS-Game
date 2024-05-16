using System;

using Tools.Constants;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Game.Interactables
{
    public class Door : MonoBehaviour {
        [SerializeField] private bool startClosed = true;

        private Animator _animator;
        
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            if (startClosed) {
                Close();
            } else {
                Open();
            }
        }

        public void Open() {
            _animator.SetBool(AnimationConstants.Door.Open, true);
            Debug.Log("opening");
        }

        public void Close() {
            _animator.SetBool(AnimationConstants.Door.Open, false);
        }
        
        
        // [SerializeField] private SpriteRenderer spriteRenderer;
        // [SerializeField] private float closedOpacity = .3f;
        // [SerializeField] private ShadowCaster2D shadowCaster2D;
        // [SerializeField] private Collider2D objectCollider;
        // [SerializeField] private Light2D light2D;
        //
        // public void Opened() {
        //     Color c = spriteRenderer.color;
        //     spriteRenderer.color = new Color(c.r, c.g, c.b, closedOpacity);
        //     shadowCaster2D.enabled = false;
        //     objectCollider.enabled = false;
        //     light2D.enabled = false;
        // }
    }
}
