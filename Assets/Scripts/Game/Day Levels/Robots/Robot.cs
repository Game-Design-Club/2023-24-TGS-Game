using System;
using Game.Day_Levels.Robots.Robot_Paths;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots
{
    public class Robot : MonoBehaviour, IComparable<Robot>
    {
        private static float _hardStopDistance = 1f;
        [SerializeField] private LayerMask layerMask;
        public RobotPath path;
        [HideInInspector] public float dstAlongPath = 0;
        [HideInInspector]public RobotPathPoint destination;
        [HideInInspector]public float velocity;

        public float distanceUntilCollision = 0;

        private void OnEnable()
        {
            RobotPath.SegmentInfo currentSegment = path.GetSegment(dstAlongPath);
            UpdatePosition(currentSegment);
        }

        private void Update()
        {
            CalculateDistanceTillCollision();

            RobotPath.SegmentInfo currentSegment = path.GetSegment(dstAlongPath);
            
            UpdatePosition(currentSegment);
            
            velocity = distanceUntilCollision < _hardStopDistance && distanceUntilCollision < Vector2.Distance(transform.position, destination.position) ? 0 : currentSegment.Speed;

            dstAlongPath = (dstAlongPath + velocity * Time.deltaTime) % path.length;
            
            // Quaternion newRotation = Quaternion.LookRotation(GetDirection());
            // Quaternion newRotation2D = Quaternion.Euler(0f, 0f, newRotation.eulerAngles.y);
            // transform.rotation = newRotation2D;

        }

        public void UpdatePosition(RobotPath.SegmentInfo currentSegment)
        {
            float percent = (dstAlongPath - currentSegment.StartDstFromStart) / currentSegment.Length;
            transform.position = Vector2.Lerp(currentSegment.Start.position, currentSegment.End.position, percent);
            destination = currentSegment.End;
        }
        
        private void CalculateDistanceTillCollision()
        {
            Vector2 direction = GetDirection();
            Vector2 perp = Vector2.Perpendicular(direction) * 0.5f;
            Vector2 startPosition = (Vector2)transform.position;
            distanceUntilCollision = SendRay(startPosition, direction);
            distanceUntilCollision = Mathf.Min(distanceUntilCollision, SendRay(startPosition + perp, direction));
            distanceUntilCollision = Mathf.Min(distanceUntilCollision, SendRay(startPosition - perp, direction));
        }

        public float SendRay(Vector2 startingPosition, Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                startingPosition,
                direction,
                Mathf.Infinity,
                layerMask);
            if (hit.collider is null) {
                Debug.Log(startingPosition);
                Debug.Log(direction);
                Debug.LogWarning("Robot collider hit nothing.", this);
                distanceUntilCollision = float.PositiveInfinity;
            }
            return hit.distance;
        }

        public Vector2 GetDirection()
        {
            return (destination.position - (Vector2)transform.position).normalized;
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

        public int CompareTo(Robot other)
        {
            return dstAlongPath.CompareTo(other.dstAlongPath);
        }
    }
}
