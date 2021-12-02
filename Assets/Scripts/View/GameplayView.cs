using UnityEngine;
using View.Components;
using View.Input;

namespace View
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private FollowCamera _followCamera;
        [SerializeField] private KeyboardInput _keyboardInput;
        [SerializeField] private Transform _viewPoint;
        [SerializeField] private LevelMap _levelMap;
        
        public Transform[] SpawnPoints => _levelMap.SpawnPoints;
        public Transform[] BoosterSpawnPoints => _levelMap.BoosterSpawnPoints;

        public void FollowPlayer(Transform player)
        {
            _followCamera.SetTarget(player);
        }
        
        public void SetLocalPlayer(PlayerController playerController)
        {
            FollowPlayer(playerController.transform);
            _keyboardInput.SetTarget(playerController.transform);
            playerController.SetInput(_keyboardInput);
        }

        public void AddPlayer(PlayerController playerController)
        {
            playerController.HitpointsView.SetRotationConstraint(_viewPoint);
        }
    }
}