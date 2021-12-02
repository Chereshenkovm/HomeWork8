using System;
using Photon.Pun;
using UnityEngine;

namespace View.Components
{
    public class CharacteristicsSynchronization : MonoBehaviour, IPunObservable
    {
        private PlayerController _playerController;

        private bool _firstData = true;
        private float speed = 5f;

        private void OnEnable()
        {
            _playerController = GetComponent<PlayerController>();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playerController.Speed);
            }
            else
            {
                speed = (float)stream.ReceiveNext();

                if (_firstData)
                {
                    _playerController.Speed = speed;
                    _firstData = false;
                }
            }
        }
    }
}
