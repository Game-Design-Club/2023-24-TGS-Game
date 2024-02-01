using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots.Robot_Paths
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
                float idealDst = idealPositions[i];
                // robots[i].idealDst = idealDst;
                RobotPath.SegmentInfo segmentInfo = path.GetSegment(idealDst);
                idealPositions[i] = (idealPositions[i] + segmentInfo.Speed * Time.deltaTime) % path.length;
            }
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
