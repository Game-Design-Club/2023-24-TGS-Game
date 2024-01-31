using System;
using Game.Day_Levels.Robots.Robot_Paths;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots
{
    public class Robot : MonoBehaviour
    {
        private static float _robotSpeed = 5f;
        public RobotPath path;
        private float _dstAlongPath = 0;

        private void Update()
        {
            _dstAlongPath = (_dstAlongPath + _robotSpeed * Time.deltaTime) % path.length;
            
            int numPoints = path.points.Count;

            float currentDst = 0;
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint point = path.points[i];
                RobotPathPoint next = path.points[(i + 1) % numPoints];
                float dst = Vector2.Distance(point.position, next.position);
                
                if (currentDst + dst < _dstAlongPath)
                {
                    currentDst += dst;
                    continue;
                }
                
                //found current segment
                float percent = (_dstAlongPath - currentDst) / dst;
                transform.position = Vector2.Lerp(point.position, next.position, percent);
                break;
            }
        }
    }
}
