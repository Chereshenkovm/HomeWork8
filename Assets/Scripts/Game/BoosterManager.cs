using System;
using Game.Configs;
using System.Collections;
using UnityEngine;
using View;
using Photon.Pun;
using View.Components;
using Random = UnityEngine.Random;

namespace Game
{
    public class BoosterManager
    {
        public event Action<GameObject, BonusManager.BonusEffect> OnTargetReachedEvent;
        
        private readonly NetworkManager _networkManager;
        private readonly GameConfig _config;
        private readonly GameplayView _gameplayView;

        private float timePassed = 0f;
        public BoosterManager(GameConfig config, NetworkManager networkManager, GameplayView gameplayView)
        {
            _config = config;
            _networkManager = networkManager;
            _gameplayView = gameplayView;
        }
        
        private void CreateBooster()
        {
            var points = _gameplayView.BoosterSpawnPoints;
            
            var boosterSpawnPoint = points[Random.Range(0, points.Length)];
            switch (Random.Range(0, 2))
            {
                case 0:
                    var booster1 = PhotonNetwork.Instantiate(_config.BoosterPrefab.Path, boosterSpawnPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
                    booster1.GetComponent<BoosterController>().BoosterPickedEvent += OnTargetReached;

                    break;
                case 1:
                    var booster2 = PhotonNetwork.Instantiate(_config.BoosterPrefabFreeze.Path, boosterSpawnPoint.position + new Vector3(0, 1, 0), Quaternion.identity);
                    booster2.GetComponent<BoosterController>().BoosterPickedEvent += OnTargetReached;
                    break;
            }

        }

        public void Tick(float deltatime)
        {   
            if (timePassed > 1)
            {
                if(Random.Range(0,10)== 9)
                    CreateBooster();
                timePassed = 0;
            }

            timePassed += deltatime;
        }
        
        private void OnTargetReached(BoosterController booster, GameObject player)
        {
            BonusManager.BonusEffect bonus = booster._bonusEffect;
            Remove(booster);
            OnTargetReachedEvent?.Invoke(player, bonus);
        }

        private void Remove(BoosterController booster)
        {
            booster.BoosterPickedEvent -= OnTargetReached;
            PhotonNetwork.Destroy(booster.gameObject);
        }
    }
}
