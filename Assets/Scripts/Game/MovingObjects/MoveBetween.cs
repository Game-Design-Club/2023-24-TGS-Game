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
        [SerializeField] private bool startMovingOnStart = true;
        private int _currentPointIndex;
        private Vector2 _currentPoint;
        private Vector2 _nextPoint;
        private float _distance;
        private float _startTime;
        private float _elapsedTime;

        private bool _movingBackwards = false;
        private bool _isMoving = false;

        private Vector2[] _points;
        
        private LineRenderer _lineRenderer;
    
        private void OnEnable() {
            GameManagerEvents.OnLevelStart += OnLevelStart;
        }
    
        private void OnDisable() {
            GameManagerEvents.OnLevelStart -= OnLevelStart;
        }
        
        private void Awake() {
            _lineRenderer = pointsParent.GetComponent<LineRenderer>();
        }

        private void OnLevelStart() {
            _isMoving = startMovingOnStart;
            StartCoroutine(StartAfterDelay());
        }
        
        private IEnumerator StartAfterDelay() {
            StartMoving();
            yield return new WaitForSeconds(startDelay);
            SetActive(_isMoving);
        }

        private void Update() {
            if (!_isMoving) return;

            _elapsedTime += Time.deltaTime;
            float distanceCovered = _elapsedTime * speed;
            float fractionOfJourney = distanceCovered / _distance;

            transform.position = Vector2.Lerp(_currentPoint, _nextPoint, fractionOfJourney);

            if ((Vector2)transform.position != _nextPoint) return;

            if (_currentPointIndex >= _points.Length - 1) {
                switch (loop) {
                    case LoopType.None:
                        _isMoving = false;
                        return;
                    case LoopType.Cyclical:
                        _currentPoint = _nextPoint;
                        _currentPointIndex = 0;
                        _nextPoint = _points[_currentPointIndex];
                        break;
                    case LoopType.Linear:
                        transform.position = _points[0];
                        _currentPoint = _points[0];
                        _currentPointIndex = 0;
                        _nextPoint = _points[_currentPointIndex += 1];
                        break;
                    case LoopType.PingPong:
                        _movingBackwards = true;
                        _currentPoint = _nextPoint;
                        _currentPointIndex -= 1;
                        _nextPoint = _points[_currentPointIndex];
                        break;
                }
            } else if (loop == LoopType.PingPong && _currentPointIndex <= 0) {
                _movingBackwards = false;
                _currentPoint = _nextPoint;
                _currentPointIndex += 1;
                _nextPoint = _points[_currentPointIndex];
            } else {
                _currentPoint = _nextPoint;
                _currentPointIndex += _movingBackwards ? -1 : 1;
                _nextPoint = _points[_currentPointIndex];
            }

            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _elapsedTime = 0;
        }
        
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
            _elapsedTime = 0;

            if (showLine && _lineRenderer != null) {
                if (loop == LoopType.Cyclical && _points.Length > 2) {
                    _lineRenderer.loop = true;
                }
                Vector3[] lineRendererPoints = new Vector3[_points.Length];
                for (int i = 0; i < _points.Length; i++) {
                    lineRendererPoints[i] = new Vector3(_points[i].x, _points[i].y, 0);
                }
                _lineRenderer.positionCount = _points.Length;
                _lineRenderer.SetPositions(lineRendererPoints);
            }

            if (_lineRenderer == null) {
                Debug.LogWarning("LineRenderer not found");
            }
        }
        
        public void SetActive(bool active) {
            _isMoving = active;
        }
        
        public void SwitchActive() {
            SetActive(!_isMoving);
        }

        public void ChangeSpeed(float newSpeed) {
            if (newSpeed <= 0) return;

            float fractionOfJourney = _elapsedTime * speed / _distance;
            _currentPoint = Vector2.Lerp(_currentPoint, _nextPoint, fractionOfJourney);

            speed = newSpeed;
            _distance = Vector2.Distance(_currentPoint, _nextPoint);
            _elapsedTime = 0;
        }
    }
}