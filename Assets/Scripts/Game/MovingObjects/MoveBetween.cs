using System.Collections;

using Game.GameManagement;
using Game.NightLevels.LaserShooters;

using UnityEngine;

namespace Game.MovingObjects
{
    public class MoveBetween : MonoBehaviour {
        [SerializeField] private GameObject pointsParent;
        [SerializeField] private LoopType loop = LoopType.Cyclical;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float startDelay = 0f;
        [SerializeField] private bool showLine = true;
    
        private int _currentPointIndex;
        private Vector2 _currentPoint;
        private Vector2 _nextPoint;
        private float _distance;
        private float _startTime;

        private bool _movingBackwards = false;
        private bool _isMoving = false;

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
            StartCoroutine(StartAfterDelay());
        }
        private IEnumerator StartAfterDelay() {
            yield return new WaitForSeconds(startDelay);
            StartMoving();
        }
        private void Update() {
            if (!_isMoving) return;
            float distanceCovered = (Time.time - _startTime) * speed;
            float fractionOfJourney = distanceCovered / _distance;
            if (fractionOfJourney > 1) fractionOfJourney = 1;
            transform.position = Vector2.Lerp(_currentPoint, _nextPoint, fractionOfJourney);
            if ((Vector2)transform.position != _nextPoint) { return; }
            // Reached the next point
            if (_currentPointIndex >= _points.Length - 1) {
                // Reached the last point
                switch (loop) {
                    case LoopType.None:
                        _isMoving = false;
                        return;
                    case LoopType.Cyclical:
                        // Move back to the first point
                        _currentPoint = _nextPoint;
                        _currentPointIndex = 0;
                        _nextPoint = _points[_currentPointIndex];
                        break;
                    case LoopType.Linear:
                        // Snap to the first point (no movement between them, but move after that)
                        transform.position = _points[0];
                        _currentPoint = _points[0];
                        _currentPointIndex = 0;
                        _nextPoint = _points[_currentPointIndex += 1];
                        break;
                    case LoopType.PingPong:
                        // Start moving backwards
                        _movingBackwards = true;
                        _currentPoint = _nextPoint;
                        _currentPointIndex -= 1;
                        _nextPoint = _points[_currentPointIndex];
                        break;
                    
                }
            } else if (LoopType.PingPong == loop && _currentPointIndex <= 0) {
                // Reached the first point
                // Start moving forwards
                _movingBackwards = false;
                _currentPoint = _nextPoint;
                _currentPointIndex += 1;
                _nextPoint = _points[_currentPointIndex];
            } else {
                // Move to the next point
                _currentPoint = _nextPoint;
                if (_movingBackwards) {
                    _currentPointIndex -= 1;
                } else {
                    _currentPointIndex += 1;
                }
                _nextPoint = _points[_currentPointIndex];
            }
            
            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _startTime = Time.time;
        }
        
        // Public functions
        public void StartMoving() {
            _points = new Vector2[pointsParent.transform.childCount];
            for (int i = 0; i < _points.Length; i++) {
                _points[i] = pointsParent.transform.GetChild(i).transform.position;
            }
            if (_points.Length <= 1) {
                enabled = false;
                return;
            }
            _currentPointIndex = 0;
            _currentPoint = _points[_currentPointIndex];
            _nextPoint = _points[_currentPointIndex += 1];
            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _startTime = Time.time;


            if (showLine && _lineRenderer != null) {
                if (loop == LoopType.Cyclical && _points.Length > 2) { // If there are only 2 points, it's a straight line
                    _lineRenderer.loop = true;
                }
                Vector3[] lineRendererPoints = new Vector3[_points.Length];
                for (int i = 0; i < _points.Length; i++) {
                    lineRendererPoints[i] = new Vector3(_points[i].x, _points[i].y, 0);
                }
                _lineRenderer.positionCount = _points.Length;
                _lineRenderer.SetPositions(lineRendererPoints);
            }
            
            _isMoving = true;
        }
        
        public void StopMoving() {
            _isMoving = false;
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
