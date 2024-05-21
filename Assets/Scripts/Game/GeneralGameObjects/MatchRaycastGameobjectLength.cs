using System;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.GeneralGameObjects {
    public class MatchRaycastGameobjectLength : MonoBehaviour {
        // Matches the length of the game objects to the raycast length
        // Used for things like lasers or fans that need to continue until they hit a wall
        
        [SerializeField] private float maxDistance = Single.PositiveInfinity;
        [SerializeField] private GameObject[] objectsToAdjustPosition;
        [SerializeField] private BoxCollider2D[] boxColliderToMatchLength;
        [SerializeField] private Light2D[] lightsToMatchLength;
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

            float height = .5f;
            float width = hit.distance - 0.5f;
            Vector3 startPoint = transform.position;
            Vector3 endPoint = transform.position + transform.right * hit.distance;
            Vector3 midPoint = (startPoint + endPoint) / 2 + transform.right * .25f;
            Vector3 rightPoint = endPoint - startPoint;
            
            
            // Vector3 currentScale = currentComponent.transform.localScale;
            // currentComponent.transform.localScale = new Vector3(hit.distance, currentScale.y, currentScale.z);
            // Vector3 startPoint = transform.position;
            // Vector3 endPoint = transform.position + transform.right * hit.distance;
            // currentComponent.transform.position = (startPoint + endPoint) / 2;
            // currentComponent.transform.right = endPoint - startPoint;
            
            foreach (GameObject currentObject in objectsToAdjustPosition)
            {
                currentObject.transform.position = midPoint;
                currentObject.transform.right = rightPoint;
            }

            foreach (BoxCollider2D currentBox in boxColliderToMatchLength)
            {
                currentBox.size = new Vector2(width, height);
            }

            Vector3[] freeformPoints = new Vector3[]
            {
                new Vector3(width / 2f, height / 4f, 0f),
                new Vector3(-width / 2f, height / 4f, 0f),
                new Vector3(-width / 2f, -height / 4f, 0f),
                new Vector3(width / 2f, -height / 4f, 0f)
            };
            foreach (Light2D currentLight in lightsToMatchLength)
            {
                currentLight.SetShapePath(freeformPoints);;
            }
        }
    }
}