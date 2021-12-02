using UnityEngine;

namespace View.Input
{
    public class KeyboardInput : MonoBehaviour, IPlayerInput
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Transform _target;
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            if (_target == null)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }
            
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            var position = _target.position;
            var viewDirection = plane.Raycast(ray, out var distance)
                ? Quaternion.LookRotation(ray.GetPoint(distance) - position)
                : Quaternion.identity;

            var mousePressed = UnityEngine.Input.GetButton("Fire1");
            var moveVector = mousePressed
                ? (ray.GetPoint(distance) - position).normalized
                : new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
            
            return (
                moveVector,
                viewDirection,
                UnityEngine.Input.GetButtonDown("Jump") || UnityEngine.Input.touchCount > 1);
        }
    }
}