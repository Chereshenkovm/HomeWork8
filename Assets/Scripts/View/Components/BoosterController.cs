using System;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace View.Components
{
    public class BoosterController : MonoBehaviour
    {
        public event Action<BoosterController, GameObject> BoosterPickedEvent;
        public BonusManager.BonusEffect _bonusEffect;


        private void OnEnable()
        {
            //_bonusEffect = BonusManager.BonusEffect.Freeze;
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.gameObject.GetComponentInParent<TransformSynchronization>();
            if(target != null)
            {
                BoosterPickedEvent?.Invoke(this, other.gameObject);
            }
        }
    }
}
