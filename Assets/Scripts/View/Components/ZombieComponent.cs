using UnityEngine;

namespace View.Components
{
    public class ZombieComponent : MonoBehaviour, IBulletTarget
    {
        [SerializeField] private GameObject _aliveView;

        [SerializeField] private GameObject _diedView;

        [SerializeField] private float _speed = 5f;

        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Vector3[] _deltaPath;

        private int _currentPoint = 0;
        private Vector3 _initPosition;

        private void Awake()
        {
            _initPosition = transform.position;
        }

        private void OnEnable()
        {
            SetState(true);
        }

        private void Update()
        {
            if (_deltaPath == null || _deltaPath.Length < 2)
                return;

            var direction = _initPosition + _deltaPath[_currentPoint] - transform.position;
            _rigidbody.velocity = IsAlive ? direction.normalized * _speed : Vector3.zero;

            if (direction.magnitude <= 0.1f)
            {
                _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
            }
        }

        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);
        }

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}