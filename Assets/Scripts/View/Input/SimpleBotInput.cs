using System.Linq;
using UnityEngine;

namespace View.Input
{
    public class SimpleBotInput : MonoBehaviour, IPlayerInput
    {
        [SerializeField] private ZombieMap _zombieMap;
        [SerializeField] private Transform _player;
        [SerializeField] private float _fireDistance;

        public (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var alivePositions = _zombieMap.AlivePositions();
            if (alivePositions.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }

            var target = alivePositions.First();
            var direction = (target - _player.position);

            return (direction, Quaternion.LookRotation(direction), direction.magnitude <= _fireDistance);
        }
    }
}