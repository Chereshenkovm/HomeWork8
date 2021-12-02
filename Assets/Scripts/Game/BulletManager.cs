using System;
using System.Collections.Generic;
using Game.Configs;
using Photon.Pun;
using UnityEngine;
using View.Components;

namespace Game
{
    public class BulletManager
    {
        public event Action<IBulletTarget> OnTargetReachedEvent;
        
        private readonly GameConfig _config;

        private readonly Dictionary<BulletController, BulletState> _playerBullets =
            new Dictionary<BulletController, BulletState>();

        private readonly List<BulletController> _bulletsTmp = new List<BulletController>();

        public BulletManager(GameConfig config)
        {
            _config = config;
        }

        public void CreateBullet(Vector3 point, Quaternion rotation)
        {
            var bullet = PhotonNetwork.Instantiate(_config.BulletPrefab.Path, point, rotation).GetComponent<BulletController>();
            bullet.TargetReachedEvent += OnTargetReached;
            _playerBullets[bullet] = new BulletState(_config.BulletLifeTime);
        }

        public void Tick(float deltaTime)
        {
            foreach (var pair in _playerBullets)
            {
                pair.Value.Tick(deltaTime);
                if (!pair.Value.IsAlive)
                {
                    _bulletsTmp.Add(pair.Key);
                }
            }

            foreach (var controller in _bulletsTmp)
            {
                Remove(controller);
            }
            
            _bulletsTmp.Clear();
        }
        
        private void OnTargetReached(BulletController bullet, IBulletTarget target)
        {
            Remove(bullet);
            OnTargetReachedEvent?.Invoke(target);
        }

        private void Remove(BulletController bullet)
        {
            bullet.TargetReachedEvent -= OnTargetReached;
            _playerBullets.Remove(bullet);
            PhotonNetwork.Destroy(bullet.gameObject);
        }
    }
}