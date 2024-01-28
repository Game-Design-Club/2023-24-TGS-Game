using UnityEditor;

using UnityEngine;

namespace Tools.Editor
{
    [CustomEditor(typeof(SpriteRenderer))]
    public class SnapForSpriteRenderer : UnityEditor.Editor
    {
        SpriteRenderer _spriteRenderer;
        Event _event;
        void OnEnable() {
            _spriteRenderer = target as SpriteRenderer;
        }
        
        private void OnSceneGUI() {
            _event = Event.current;

            if (_event.type != EventType.MouseUp) return;
            if (!_event.control) return;

            float xSize = Mathf.Round(_spriteRenderer.size.x);
            float ySize = Mathf.Round(_spriteRenderer.size.y);
            
            if (xSize < 1) xSize = 1;
            if (ySize < 1) ySize = 1;
            
            _spriteRenderer.size = new Vector2(xSize, ySize);
            
            Transform targetTransform = _spriteRenderer.transform;
            
            Undo.RecordObject(targetTransform, "Snap to Grid");
            
            Vector3 position = targetTransform.position;
            position = new Vector3(
                Mathf.Round(2*position.x)/2,
                Mathf.Round(2*position.y)/2,
                position.z
            );
            _spriteRenderer.transform.position = position;
            Vector3 localScale = targetTransform.localScale;
            targetTransform.localScale = new Vector3(
                Mathf.Round(localScale.x),
                Mathf.Round(localScale.y),
                localScale.z
            );
            
            EditorUtility.SetDirty(targetTransform);
        }
    }
}