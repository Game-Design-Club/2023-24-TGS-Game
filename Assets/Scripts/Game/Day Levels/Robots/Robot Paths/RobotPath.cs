using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots.Robot_Paths
{
    [System.Serializable]
    public class RobotPath
    {
        public List<RobotPathPoint> points;
        [HideInInspector] public float length;

        public RobotPath(Vector2 center)
        {
            points = new List<RobotPathPoint>();
            AddPoint(center - Vector2.left);
            AddPoint(center - Vector2.right);
            CalculateLength();
        }


        public void AddPoint(Vector2 position)
        {
            points.Add(new RobotPathPoint(new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y))));
            CalculateLength();
        }

        public void MovePoint(int index, Vector2 position)
        {
            points[index].position = new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            CalculateLength();
        }

        public void CalculateLength()
        {
            length = 0;
            int numPoints = points.Count;
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint start = points[i];
                RobotPathPoint end = points[(i + 1) % numPoints];
                length += Vector2.Distance(start.position, end.position);
            }
        }

    }
}
