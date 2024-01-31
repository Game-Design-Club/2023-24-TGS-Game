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

        private void OnEnable()
        {
            GameObject robotObject = Instantiate(robotPrefab, transform.position, Quaternion.identity);
            Robot robot = robotObject.GetComponent<Robot>();
            robot.path = path;
        }
    }
}
