using System;
using Game.Day_Levels.Robots.Robot_Paths;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots
{
    public class Robot : MonoBehaviour
    {
        private static float _robotSpeed = 5f;
        private static float _hardStopDistance = 1f;
        [SerializeField] private LayerMask layerMask;
        public RobotPath path;
        [HideInInspector] public float dstAlongPath = 0;
        [HideInInspector]public RobotPathPoint destination;
        [HideInInspector]public float velocity = _robotSpeed;

        public float distanceUntilCollision = 0;

        private void OnEnable()
        {
            UpdatePosition();
        }

        private void Update()
        {
            CalculateDistanceTillCollision();
            velocity = distanceUntilCollision < _hardStopDistance ? 0 : _robotSpeed;
            
            dstAlongPath = (dstAlongPath + velocity * Time.deltaTime) % path.length;
            
            UpdatePosition();
            // Quaternion newRotation = Quaternion.LookRotation(GetDirection());
            // Quaternion newRotation2D = Quaternion.Euler(0f, 0f, newRotation.eulerAngles.y);
            // transform.rotation = newRotation2D;

        }

        public void UpdatePosition()
        {
            int numPoints = path.points.Count;
            float currentDst = 0;
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint start = path.points[i];
                RobotPathPoint end = path.points[(i + 1) % numPoints];
                float dst = Vector2.Distance(start.position, end.position);
                
                if (currentDst + dst < dstAlongPath)
                {
                    currentDst += dst;
                    continue;
                }
                
                //found current segment
                float percent = (dstAlongPath - currentDst) / dst;
                transform.position = Vector2.Lerp(start.position, end.position, percent);
                destination = end;
                break;
            }
        }
        
        private void CalculateDistanceTillCollision()
        {
            Vector2 direction = GetDirection();
            float minDistance = MinDistance();
            Vector2 startPosition = (Vector2)transform.position;
            RaycastHit2D hit = Physics2D.Raycast(
                startPosition,
                direction,
                Mathf.Infinity,
                layerMask);
            if (hit.collider is null) {
                Debug.LogWarning("Laser hit nothing.", this);
                distanceUntilCollision = float.PositiveInfinity;
            }

            distanceUntilCollision = hit.distance;
        }

        public Vector2 GetDirection()
        {
            return (destination.position - (Vector2)transform.position).normalized;
        }

        public float MinDistance()
        {
            return .5f / Mathf.Cos(90 - Vector2.Angle(transform.position, destination.position));
        }

        public void SetPosition(Vector2 referencePoint)
        {
            int numPoints = path.points.Count;
            
            float totalDstOnPath = 0;
            float smallestDstOnPath = 0;
            
            float smallestDstFromLine = float.PositiveInfinity;
            int indexOfSmallest = 0;
            for (int i = 0; i < numPoints; i++)
            {
                Vector2 start = path.points[i].position;
                Vector2 end = path.points[(i + 1) % numPoints].position;
                float dst = HandleUtility.DistancePointLine(referencePoint, start, end);
                if (dst < smallestDstFromLine)
                {
                    smallestDstFromLine = dst;
                    indexOfSmallest = i;
                    smallestDstOnPath = totalDstOnPath;
                }

                totalDstOnPath += Vector2.Distance(start, end);
            }
            Vector2 shortestStart = path.points[indexOfSmallest].position;
            Vector2 shortestEnd = path.points[(indexOfSmallest + 1) % numPoints].position;
            
            Vector2 newPos = (Vector2)HandleUtility.ProjectPointLine(referencePoint,shortestStart, shortestEnd);            
            transform.position = newPos;
            
            float subSegLength = Vector2.Distance(shortestStart, newPos);
            dstAlongPath = smallestDstOnPath + subSegLength;
        }
    }
}
