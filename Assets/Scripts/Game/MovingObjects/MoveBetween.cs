using System.Linq;

using Game.GameManagement;

using UnityEngine;

namespace Game.MovingObjects
{
    public class MoveBetween : MonoBehaviour {
        [SerializeField] private GameObject pointsParent;
        [SerializeField] private float speed = 3f;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool showLine = true;
    
        private int _currentPointIndex;
        private Vector2 _currentPoint;
        private Vector2 _nextPoint;
        private float _distance;
        private float _startTime;

        private Vector2[] _points;
        
        private LineRenderer _lineRenderer;
    
        // Unity functions
        private void OnEnable() {
            GameManager.Instance.OnLevelStart += OnLevelStart;
        }
    
        private void OnDisable() {
            GameManager.Instance.OnLevelStart -= OnLevelStart;
        }
        
        private void Awake() {
            _lineRenderer = pointsParent.GetComponent<LineRenderer>();
        }
        // Private functions
        private void OnLevelStart() {
            _points = new Vector2[pointsParent.transform.childCount];
            for (int i = 0; i < _points.Length; i++) {
                _points[i] = pointsParent.transform.GetChild(i).transform.position;
            }
            if (_points.Length <= 1) {
                enabled = false;
            }

            _currentPointIndex = 0;
            _currentPoint = _points[_currentPointIndex];
            _nextPoint = _points[_currentPointIndex += 1];
            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _startTime = Time.time;


            if (showLine && _lineRenderer != null) {
                Vector3[] lineRendererPoints = new Vector3[_points.Length];
                for (int i = 0; i < _points.Length; i++) {
                    lineRendererPoints[i] = new Vector3(_points[i].x, _points[i].y, 0);
                }
                _lineRenderer.positionCount = _points.Length;
                _lineRenderer.SetPositions(lineRendererPoints);
            }
        }
    
        private void Update() {
            float distanceCovered = (Time.time - _startTime) * speed;
            float fractionOfJourney = distanceCovered / _distance;
            if (fractionOfJourney > 1) fractionOfJourney = 1;
            transform.position = Vector2.Lerp(_currentPoint, _nextPoint, fractionOfJourney);
            if ((Vector2)transform.position != _nextPoint) { return; }
            // Reached the next point
            _currentPoint = _nextPoint;
            if (_currentPointIndex >= _points.Length - 1) {
                if (loop) {
                    _currentPointIndex = -1;
                } else {
                    enabled = false;
                    return;
                }
            }

            _currentPointIndex++;
            _nextPoint = _points[_currentPointIndex];

            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _startTime = Time.time;
        }
    }
}



/*
 * POINT (1, 5) [0]
 * POINT (10, 50) [1]
 * POINT (100, 500) [2]
 *
 * CURRENT INDEX: 0
 *
 * WHEN REACHED:
 * CURRENT POINT: LAST NEXT POINT (if first 0)
 * NEXT POINT: CURRENT INDEX += 1
 *
 * WHEN REACHED:
 * CURRENT POINT: LAST NEXT POINT
 * NEXT POINT: CURRENT INDEX += 1
 *
 * WHEN REACHED:
 * CURRENT POINT: LAST NEXT POINT
 * NEXT POINT: CURRENT INDEX += 1
 *
 * WHEN REACHED:
 * CURRENT POINT: LAST NEXT POINT
 * NEXT POINT: (IF LAST, CURRENT INDEX = -1) CURRENT INDEX += 1
 */
