using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Game.Robots.Robot_Paths
{
    public class RobotPathSupervisor : MonoBehaviour
    {
        [SerializeField] private GameObject robotPrefab;
        public RobotPath path = null;
        public List<Robot> robots = new List<Robot>();
        public List<float> idealPositions;

        private void OnEnable()
        {
            robots.Sort();
            idealPositions = new List<float>();
            foreach (Robot robot in robots)
            {
                idealPositions.Add(robot.dstAlongPath);
            }
        }

        private void Update()
        {
            for (int i = 0; i < robots.Count; i++)
            {
                RobotPath.SegmentInfo segmentInfo = path.GetSegment(idealPositions[i]);
                idealPositions[i] = (idealPositions[i] + segmentInfo.Speed * Time.deltaTime) % path.length;
            }
            
            robots.Sort();
            idealPositions.Sort();

            float smallestDistance = float.PositiveInfinity;
            int smallestIndexShift = 0;
            for (int indexShift = 0; indexShift < idealPositions.Count; indexShift++)
            {
                float totalDistance = 0;
                for (int robotIndex = 0; robotIndex < robots.Count; robotIndex++)
                {
                    float dst = Distance(robots[robotIndex].dstAlongPath,
                        idealPositions[(robotIndex + indexShift) % idealPositions.Count]);
                    totalDistance += Mathf.Min(dst, path.length - dst);
                }

                if (totalDistance < smallestDistance)
                {
                    smallestDistance = totalDistance;
                    smallestIndexShift = indexShift;
                }
            }
            
            for (int i = 0; i < robots.Count; i++)
            {
                robots[i].idealDst =
                    idealPositions[(i + smallestIndexShift) % idealPositions.Count];
            }
            
            
            
            // float distanceFromIdealAhead = 0f;
            // float distanceFromIdealBehind = 0f;
            // for (int i = 0; i < robots.Count; i++)
            // {
            //     float distance = Distance(idealPositions[i])
            //     distanceFromIdealAhead += Mathf.Abs(idealPositions[i] - robots[i].dstAlongPath);
            //     distanceFromIdealBehind += Mathf.Abs(idealPositions[(i - 1 + idealPositions.Count) % idealPositions.Count] - robots[i].dstAlongPath);
            // }
            //
            // int indexAdjustment = distanceFromIdealAhead < distanceFromIdealBehind ? 0 : -1;
            //
            // for (int i = 0; i < robots.Count; i++)
            // {
            //     robots[i].idealDst =
            //         idealPositions[(i + indexAdjustment + idealPositions.Count) % idealPositions.Count];
            // }


        }

        private float Distance(float distance1, float distance2)
        {
            return Mathf.Abs(distance2 - distance1);
        }
        

        private void OnValidate()
        {
            path ??= new RobotPath(transform.position);
        }

        private void Reset()
        {
            path = new RobotPath(transform.position);
        }

        public void MovePoint(int index, Vector2 position)
        {
            path.MovePoint(index, position);
            foreach (Robot robot in robots)
            {
                robot.path = path;
                robot.SetPosition(robot.transform.position);
            }
        }

        public void UpdateRobotList()
        {
            robots.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                Robot robot = transform.GetChild(i).GetComponent<Robot>();
                if (robot != null)
                {
                    robot.path = path;
                    robots.Add(robot);
                }
            }
        }

        public void AddRobot()
        {
            GameObject robotObject = PrefabUtility.InstantiatePrefab(robotPrefab, transform) as GameObject;
            robotObject.transform.position = path.points[0].position;
            
            Robot robot = robotObject.GetComponent<Robot>();
            robot.path = path;
            robots.Add(robot);
        }
    }
}
