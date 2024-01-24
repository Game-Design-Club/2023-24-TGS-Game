using Constants;

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
 
        private void OnSceneGUI(){
            _event = Event.current;

            if (_event.type != EventType.MouseUp) return;
            _spriteRenderer.size = new Vector2(Mathf.Round(_spriteRenderer.size.x), Mathf.Round(_spriteRenderer.size.y));

            Transform targetTransform = _spriteRenderer.transform;
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
 
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}