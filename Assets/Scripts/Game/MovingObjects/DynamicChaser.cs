using UnityEngine;

namespace Game.MovingObjects {
    public class DynamicChaser : MonoBehaviour {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _chaser;
        
        [SerializeField] private float _base = 1;
        [SerializeField] private float _baseDistance = 40;
        [SerializeField] private float _speed = 1;

        [SerializeField] private float _minSpeed = 5.1f;
        
        [SerializeField] private MoveBetween _moveBetween;
        
        public float GetIdealSpeed() {
            // If they are far away, move faster
            float distance = Vector3.Distance(_target.position, _chaser.position) - _baseDistance;
            float extra = distance * _speed;
            float final = _base + extra;
            if (final < _minSpeed) {
                final = _minSpeed;
            }
            Debug.Log($"Distance: {distance}, Extra: {extra}, Final: {final}");
            return final;
        }
        
        public void Move() {
            float idealSpeed = GetIdealSpeed();
            _moveBetween.ChangeSpeed(idealSpeed);
        }
    }
}