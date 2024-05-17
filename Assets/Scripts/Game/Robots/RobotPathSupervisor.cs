using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Robots
{
    public class RobotPathSupervisor : MonoBehaviour
    {
        [SerializeField] public GameObject robotPrefab;
        [SerializeField] private bool showPath = true;
        public RobotPath path = null;
        public List<Robot> robots = new List<Robot>();
        public List<float> idealPositions;

        private LineRenderer _lineRenderer;
        
        private void OnEnable()
        {
            robots.Sort();
            idealPositions = new List<float>();
            foreach (Robot robot in robots)
            {
                idealPositions.Add(robot.dstAlongPath);
            }
        }

        private void Awake() {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start() {
            if (showPath) {
                CreatePath();
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
            
            
            
            float distanceFromIdealAhead = 0f;
            float distanceFromIdealBehind = 0f;
            for (int i = 0; i < robots.Count; i++) {
                distanceFromIdealAhead += Mathf.Abs(idealPositions[i] - robots[i].dstAlongPath);
                distanceFromIdealBehind += Mathf.Abs(idealPositions[(i - 1 + idealPositions.Count) % idealPositions.Count] - robots[i].dstAlongPath);
            }
            
            int indexAdjustment = distanceFromIdealAhead < distanceFromIdealBehind ? 0 : -1;
            
            for (int i = 0; i < robots.Count; i++)
            {
                robots[i].idealDst =
                    idealPositions[(i + indexAdjustment + idealPositions.Count) % idealPositions.Count];
            }


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
        
        // Private functions
        private void CreatePath() {
            Vector3[] lineRendererPoints = new Vector3[path.points.Count + 1];
            for (int i = 0; i < path.points.Count; i++) {
                lineRendererPoints[i] = new Vector3(path.points[i].position.x, path.points[i].position.y, 0);
            }
            lineRendererPoints[path.points.Count] = new Vector3(path.points[0].position.x, path.points[0].position.y, 0);
            _lineRenderer.positionCount = path.points.Count + 1;
            _lineRenderer.SetPositions(lineRendererPoints);
        }
    }
}
