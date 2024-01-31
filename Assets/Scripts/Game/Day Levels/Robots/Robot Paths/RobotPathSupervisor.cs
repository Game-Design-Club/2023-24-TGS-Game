using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots.Robot_Paths
{
    public class RobotPathSupervisor : MonoBehaviour
    {
        [SerializeField] private GameObject robotPrefab;
        public RobotPath path = null;

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
            for (int i = 0 ; i < transform.childCount ; i++)
            {
                Robot robot = transform.GetChild(i).GetComponent<Robot>();
                if (robot != null)
                {
                    robot.SetPosition(robot.transform.position);
                }
            }
        }

        public void AddRobot()
        {
            GameObject robotObject = Instantiate(robotPrefab, path.points[0].position, Quaternion.identity, transform);
            Robot robot = robotObject.GetComponent<Robot>();
            robot.path = path;
        }
    }
}
