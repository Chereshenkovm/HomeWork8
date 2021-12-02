using System;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class SharedModel : MonoBehaviour, IPunObservable
    {
        public event Action ChangedEvent;
        private bool _hasChanges;

        public int FiremanID { get; private set; }
        public GameState GameState { get; private set; } = GameState.Wait;

        public void SetFireman(int id)
        {
            FiremanID = id;
            _hasChanges = true;
        }

        public void SetState(GameState gameState)
        {
            GameState = gameState;
            _hasChanges = true;
        }

        public void Update()
        {
            if (_hasChanges)
            {
                _hasChanges = false;
                ChangedEvent?.Invoke();
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(FiremanID);
                stream.SendNext(GameState);
            }
            else
            {
                var newId = (int) stream.ReceiveNext();
                if (newId != FiremanID)
                {
                    _hasChanges = true;
                    FiremanID = newId;
                }

                var newState = (GameState)stream.ReceiveNext();
                if (newState != GameState)
                {
                    _hasChanges = true;
                    GameState = newState;
                }

                Debug.Log("[SharedModel] Receive FiremanId");
            }
        }
    }

    public enum GameState
    {
        Wait,
        Play,
        End
    }
}