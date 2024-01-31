using System;
using Game.Day_Levels.Robots.Robot_Paths;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Day_Levels.Robots
{
    [CustomEditor(typeof(Robot))]
    public class RobotEditor : Editor
    {
        public bool pressed = false;

        private void OnSceneGUI()
        {
            Robot robot = (Robot)target;
            Event guiEvent = Event.current;

            if (guiEvent.type == EventType.MouseDrag) pressed = true;
            if (guiEvent.type == EventType.MouseUp) pressed = false;

            if (!pressed) return;
            
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
            robot.SetPosition(mousePos);
        }
    }
}
