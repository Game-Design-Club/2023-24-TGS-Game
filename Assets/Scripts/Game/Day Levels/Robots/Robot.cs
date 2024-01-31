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
        public RobotPath path;
        [HideInInspector] public float dstAlongPath = 0;

        private void Update()
        {
            dstAlongPath = (dstAlongPath + _robotSpeed * Time.deltaTime) % path.length;
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            int numPoints = path.points.Count;
            float currentDst = 0;
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint point = path.points[i];
                RobotPathPoint next = path.points[(i + 1) % numPoints];
                float dst = Vector2.Distance(point.position, next.position);
                
                if (currentDst + dst < dstAlongPath)
                {
                    currentDst += dst;
                    continue;
                }
                
                //found current segment
                float percent = (dstAlongPath - currentDst) / dst;
                transform.position = Vector2.Lerp(point.position, next.position, percent);
                break;
            }
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
