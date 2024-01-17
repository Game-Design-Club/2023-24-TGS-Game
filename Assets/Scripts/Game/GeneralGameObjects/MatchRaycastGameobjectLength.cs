using System;

using UnityEngine;

namespace Game.GeneralGameObjects {
    public class MatchRaycastGameobjectLength : MonoBehaviour{
        [SerializeField] private float maxDistance = Single.PositiveInfinity;
        [SerializeField] private GameObject[] gameObjectsToMatchLength;
        [SerializeField] private LayerMask layerMask;
        
        private float _lastDistance = 0;

        // Unity functions
        private void Update() {
            CalculateLength();
        }

        // Private functions
        private void CalculateLength() {
            Transform thisTransform = transform;
            RaycastHit2D hit = Physics2D.Raycast(
                thisTransform.position,
                thisTransform.right,
                Mathf.Infinity,
                layerMask);
            if (hit.collider is null) {
                Debug.LogWarning("Laser hit nothing.", this);
                return;
            }

            float distance = hit.distance;
            if (Math.Abs(distance - _lastDistance) < .0001) return;
            ChangeLengths(distance, hit);
        }

        private void ChangeLengths(float distance, RaycastHit2D hit) {
            _lastDistance = distance;
            if (distance > maxDistance) {
                distance = maxDistance;
            }

            foreach (GameObject currentComponent in gameObjectsToMatchLength) {
                Vector3 currentScale = currentComponent.transform.localScale;
                currentComponent.transform.localScale = new Vector3(distance, currentScale.y, currentScale.z);
                Vector3 startPoint = transform.position;
                Vector3 endPoint = hit.point;
                currentComponent.transform.position = (startPoint + endPoint) / 2;
                currentComponent.transform.right = endPoint - startPoint;
            }
        }
    }
}