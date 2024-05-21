using System;
using UnityEngine;

namespace Game.Robots
{
    public class Robot : MonoBehaviour, IComparable<Robot>
    {
        private static float hardStopDistance = 1f;
        [Range(0, 1)]
        private static float distanceImpact = 0.5f; 
        [SerializeField] private LayerMask layerMask;
        public RobotPath path;
        [HideInInspector] public float dstAlongPath = 0;
        [HideInInspector]public RobotPathPoint destination;
        [HideInInspector] public float velocity;
        public float idealDst;

        public float distanceUntilCollision = 0;

        private void OnEnable()
        {
            RobotPath.SegmentInfo currentSegment = path.GetSegment(dstAlongPath);
            velocity = currentSegment.Speed;
            UpdatePosition(currentSegment);
        }

        private void FixedUpdate()
        {
            CalculateDistanceTillCollision();

            RobotPath.SegmentInfo currentSegment = path.GetSegment(dstAlongPath);
            
            UpdatePosition(currentSegment);
            
            float idealVelocity = distanceUntilCollision < hardStopDistance && distanceUntilCollision < Vector2.Distance(transform.position, destination.position) + 0.49f ? 0 : currentSegment.Speed;

            float dst = idealDst - dstAlongPath;
            float backDst = Math.Abs(path.length - Math.Abs(dst)) * (dst / Math.Abs(dst));
            if (Mathf.Abs(backDst) < Math.Abs(dst)) dst = backDst;

            idealVelocity *= VeloMult(dst);

            velocity = Mathf.Lerp(velocity, idealVelocity, 0.5f);

            dstAlongPath = (dstAlongPath + velocity * Time.deltaTime) % path.length;
            // collider2D.enabled = velocity > 0.1f;
        }

        public float VeloMult(float dst)
        {
            return 1f / (1 + Mathf.Exp(-distanceImpact * dst)) + 0.5f;
        }

        public void UpdatePosition(RobotPath.SegmentInfo currentSegment)
        {
            float percent = (dstAlongPath - currentSegment.StartDstFromStart) / currentSegment.Length;
            Vector2 previousPosition = transform.position;
            transform.position = Vector2.Lerp(currentSegment.Start.position, currentSegment.End.position, percent);
            destination = currentSegment.End;
            
            // Calculate the direction and rotation
            Vector2 direction = (Vector2)transform.position - previousPosition;
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
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
                float dst = DistancePointLine(referencePoint, start, end);
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
            
            Vector2 newPos = ProjectPointLine(referencePoint,shortestStart, shortestEnd);            
            transform.position = newPos;
            
            float subSegLength = Vector2.Distance(shortestStart, newPos);
            dstAlongPath = smallestDstOnPath + subSegLength;
        }

        public int CompareTo(Robot other)
        {
            return dstAlongPath.CompareTo(other.dstAlongPath);
        }
        
        private float DistancePointLine(Vector2 point, Vector2 start, Vector2 end) {
            float lineLength = Vector2.Distance(start, end);
            float t = Vector2.Dot(point - start, end - start) / (lineLength * lineLength);
            t = Mathf.Clamp01(t);
            Vector2 closestPoint = start + t * (end - start);
            return Vector2.Distance(point, closestPoint);
        }

        private Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            Vector3 rhs = point - lineStart;
            Vector3 vector3 = lineEnd - lineStart;
            float magnitude = vector3.magnitude;
            Vector3 lhs = vector3;
            if ((double) magnitude > 9.999999974752427E-07)
                lhs /= magnitude;
            float num = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0.0f, magnitude);
            return lineStart + lhs * num;
        }

    }
}