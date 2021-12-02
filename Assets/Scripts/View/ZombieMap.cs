using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Components;

namespace View
{
    public class ZombieMap : MonoBehaviour
    {
        [SerializeField]
        private GameObject _root;

        private List<ZombieComponent> _zombieComponents = new List<ZombieComponent>();

        private void Awake()
        {
            _zombieComponents = _root.gameObject.GetComponentsInChildren<ZombieComponent>().ToList();
        }

        public List<Vector3> AlivePositions() => _zombieComponents
            .Where(z => z.IsAlive)
            .Select(z=>z.gameObject.transform.position)
            .ToList();
    }
}