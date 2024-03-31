using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ScrollingImage : MonoBehaviour { // Scrolls a texture on a RawImage
        [SerializeField] private RawImage image;
        [SerializeField] private Vector2 scrollSpeed = Vector2.right;
        
        // Unity functions
        private void Awake() {
            if (image == null) {
                image = GetComponent<RawImage>();
            }
            if (image == null) {
                Debug.LogWarning("No RawImage component found on ScrollingImage", this);
            }
        }

        private void Update() {
            image.uvRect = new Rect(image.uvRect.position + scrollSpeed * Time.unscaledDeltaTime, image.uvRect.size);
        }
    }
}