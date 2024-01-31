using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots.Robot_Paths
{
    [System.Serializable]
    public class RobotPathPoint
    {
        public Vector2 position;

        public RobotPathPoint(Vector2 position)
        {
            this.position = position;
        }
    }
}
