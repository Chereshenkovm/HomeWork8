using System;
using UnityEngine;
using View.Components;

namespace View
{
    [CreateAssetMenu(menuName = "NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        public event Action<PlayerController> PlayerControllerCreatedEvent;

        public void RaisePlayerControllerCreated(PlayerController playerController)
        {
            PlayerControllerCreatedEvent?.Invoke(playerController);
        }
    }
}