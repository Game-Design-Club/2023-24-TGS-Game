using UnityEditor;

using UnityEngine;

namespace Game.Robots.Robot_Paths
{
    [CustomEditor(typeof(RobotPathSupervisor))]
    public class RobotPathEditor : Editor
    {
        public RobotPathSupervisor pathSupervisor;
        private int lastKnownChildCount = 0;

        private void OnEnable()
        {
            pathSupervisor = (RobotPathSupervisor)target;
            pathSupervisor.UpdateRobotList();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Add Robot"))
            {
                pathSupervisor.AddRobot();
            }
        }

        private void OnSceneGUI()
        {
            Input();
            Draw();

            if (lastKnownChildCount != pathSupervisor.transform.childCount) pathSupervisor.UpdateRobotList();
        }
        
        void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(pathSupervisor, "Add Point");
                pathSupervisor.path.AddPoint(mousePos);
            }
        }

        private void Draw()
        {
            int numPoints = pathSupervisor.path.points.Count;
            
            //Lines
            for (int i = 0; i < numPoints; i++)
            {
                RobotPathPoint start = pathSupervisor.path.points[i];
                RobotPathPoint end = pathSupervisor.path.points[(i + 1) % numPoints];
                Handles.color = Color.green;
                Handles.DrawLine(start.position, end.position, 5f);
            }
            
            //Points
            for (int i = 0; i < numPoints; i++)
            {
                Handles.color = Color.red;
                RobotPathPoint point = pathSupervisor.path.points[i];
                
                Vector2 newPos = Handles.FreeMoveHandle(point.position, .5f, Vector2.zero,
                    Handles.CylinderHandleCap);
                if (point.position != newPos)
                {
                    Undo.RecordObject(pathSupervisor, "Move Point");
                    pathSupervisor.MovePoint(i, newPos);
                }
            }
            
            // Points
             for (int i = 0; i < pathSupervisor.robots.Count; i++)
             {
                 Handles.color = Color.cyan;
                 Robot robot = pathSupervisor.robots[i];
                 float idealPos = robot.idealDst;
                 RobotPath.SegmentInfo currentSegment = pathSupervisor.path.GetSegment(idealPos);
                 float percent = (idealPos - currentSegment.StartDstFromStart) / currentSegment.Length;
                 Vector2 position = Vector2.Lerp(currentSegment.Start.position, currentSegment.End.position, percent);
                 
                 Handles.FreeMoveHandle(position, .2f, Vector2.zero, Handles.CylinderHandleCap);
                 
                 Handles.color = Color.yellow;
                 Handles.DrawLine(position, robot.transform.position);
             }
        }
    }
}
