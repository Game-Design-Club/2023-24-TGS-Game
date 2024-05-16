using System.Collections.Generic;

using Game.Robots.Robot_Paths;

using UnityEngine;

namespace Tools.Editor.Robots.Robot_Paths
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

        public SegmentInfo GetSegment(float dstAlongPath)
        {
            SegmentInfo info = new SegmentInfo();
            int numPoints = points.Count;
            float currentDst = 0;
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint start = points[i];
                RobotPathPoint end = points[(i + 1) % numPoints];
                float dst = Vector2.Distance(start.position, end.position);
                
                if (currentDst + dst < dstAlongPath)
                {
                    currentDst += dst;
                    continue;
                }

                info.Start = start;
                info.End = end;
                info.Length = dst;
                info.StartDstFromStart = currentDst;
                info.Speed = 5f;
                return info;
            }
            Debug.LogWarning("Distance " + dstAlongPath + " not in path with length " + length);
            return info;
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

        public struct SegmentInfo
        {
            public RobotPathPoint Start;
            public RobotPathPoint End;
            public float Length;
            public float StartDstFromStart;
            public float Speed;

        }
    }
}
