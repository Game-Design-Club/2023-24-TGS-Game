using System;

using UnityEngine;

namespace Game.GeneralGameObjects {
    public class MatchRaycastGameobjectLength : MonoBehaviour {
        // Matches the length of the game objects to the raycast length
        // Used for things like lasers or fans that need to continue until they hit a wall
        
        [SerializeField] private float maxDistance = Single.PositiveInfinity;
        [SerializeField] private GameObject[] gameObjectsToMatchLength;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float width = .25f;
        
        private float _lastDistance = 0;

        // Unity functions
        private void Update() {
            CalculateLength();
        }

        // Private functions
        private void CalculateLength() {
            Transform thisTransform = transform;
            RaycastHit2D hit1 = Physics2D.Raycast(
                thisTransform.position - thisTransform.up * width,
                thisTransform.right,
                Mathf.Infinity,
                layerMask);
            RaycastHit2D hit2 = Physics2D.Raycast(
                thisTransform.position + thisTransform.up * width,
                thisTransform.right,
                Mathf.Infinity,
                layerMask);
            if (hit1.collider is null || hit2.collider is null) {
                Debug.LogWarning("Laser hit nothing.", this);
                return;
            }
            
            RaycastHit2D hit = hit2.distance < hit1.distance ? hit2 : hit1;
            
            if (Math.Abs(hit.distance - _lastDistance) < .0001) return;
            ChangeLengths(hit);
        }

        private void ChangeLengths(RaycastHit2D hit) {
            _lastDistance = hit.distance;
            if (hit.distance > maxDistance) {
                hit.distance = maxDistance;
            }

            foreach (GameObject currentComponent in gameObjectsToMatchLength) {
                Vector3 currentScale = currentComponent.transform.localScale;
                currentComponent.transform.localScale = new Vector3(hit.distance, currentScale.y, currentScale.z);
                Vector3 startPoint = transform.position;
                Vector3 endPoint = transform.position + transform.right * hit.distance;
                currentComponent.transform.position = (startPoint + endPoint) / 2;
                currentComponent.transform.right = endPoint - startPoint;
            }
        }
    }
}