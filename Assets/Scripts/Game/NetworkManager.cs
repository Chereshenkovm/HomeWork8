using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using View.Components;

namespace Game
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public event Action<bool, string> RoomJoinEvent;
        public event Action ConnectedEvent;
        public event Action ModelChangedEvent;
        public event Action SpeedChangedEvent;
        public event Action FreezedChangedEvent;

        [SerializeField] private SharedModel _sharedModel;
        [SerializeField] private BonusManager _bonusManager;

        public int FiremanId => _sharedModel.FiremanID;

        public List<int> FastId => _bonusManager.FastID;
        public List<int> FreezeId => _bonusManager.FreezeID;
        public List<int> FireUpId => _bonusManager.FireUpID;
        public GameState GameState => _sharedModel.GameState;
        public bool IsMaster => PhotonNetwork.LocalPlayer.IsMasterClient;
        
        public void Start()
        {
            _sharedModel.ChangedEvent += () =>
            {
                ModelChangedEvent?.Invoke();
            };

            _bonusManager.ChangedEventFast += () =>
            {
                UpdateSpeed();
            };

            _bonusManager.ChangedEventFreeze += () =>
            {
                UpdateFreeze();
            };
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateGame(string room)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"] = PlayerPrefs.GetString("PlayerName");
            PhotonNetwork.CreateRoom(room);
        }

        public void FindRandomRoom()
        {
            PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"] = PlayerPrefs.GetString("PlayerName");
            PhotonNetwork.JoinRandomRoom();
        }

        public PlayerController CreatePlayer(string prefabName, Vector3 position, Quaternion rotation)
        {
            var player = PhotonNetwork.Instantiate(prefabName, position, rotation);
            var playerController = player.GetComponent<PlayerController>();
            
            return playerController;
        }

        public void SetFireman(int id)
        {
            photonView.RPC(nameof(SetFiremanRpc), RpcTarget.MasterClient, id);
        }

        [PunRPC]
        private void SetFiremanRpc(int id)
        {
            _sharedModel.SetFireman(id);
        }

        public void SetPlayerSpeed(int id)
        {
            photonView.RPC(nameof(SetPlayerSpeedRPC), RpcTarget.All, id);
        }

        [PunRPC]
        private void SetPlayerSpeedRPC(int id)
        {
            _bonusManager.SetBonusEffect(id, BonusManager.BonusEffect.Fast);
        }
        
        public void SetPlayerFreeze(int id)
        {
            photonView.RPC(nameof(SetPlayerFreezeRPC), RpcTarget.All, id);
        }

        [PunRPC]
        private void SetPlayerFreezeRPC(int id)
        {
            _bonusManager.SetBonusEffect(id, BonusManager.BonusEffect.Freeze);
        }
        
        public void SetPlayerFireUp(int id)
        {
            photonView.RPC(nameof(SetPlayerFireUpRPC), RpcTarget.All, id);
        }

        [PunRPC]
        private void SetPlayerFireUpRPC(int id)
        {
            _bonusManager.SetBonusEffect(id, BonusManager.BonusEffect.FireUp);
        }

        public void DeletePlayerSpeed(int id)
        {
            photonView.RPC(nameof(DeletePlayerSpeedRPC), RpcTarget.All, id);
        }

        [PunRPC]
        private void DeletePlayerSpeedRPC(int id)
        {
            _bonusManager.DeleteBonusEffect(id, BonusManager.BonusEffect.Fast);
        }
        
        public void DeletePlayerFreeze(int id)
        {
            photonView.RPC(nameof(DeletePlayerFreezeRPC), RpcTarget.All, id);
        }

        [PunRPC]
        private void DeletePlayerFreezeRPC(int id)
        {
            _bonusManager.DeleteBonusEffect(id, BonusManager.BonusEffect.Freeze);
        }

        public void UpdateSpeed()
        {
            photonView.RPC(nameof(UpdateSpeedRpc), RpcTarget.All);
        }

        [PunRPC]
        private void UpdateSpeedRpc()
        {
            SpeedChangedEvent?.Invoke();
        }

        public void UpdateFreeze()
        {
            photonView.RPC(nameof(UpdateFreezeRPC), RpcTarget.All);
        }

        [PunRPC]
        private void UpdateFreezeRPC()
        {
            FreezedChangedEvent?.Invoke();
        }

        public void StartGame()
        {
            photonView.RPC(nameof(ChangeState), RpcTarget.MasterClient, GameState.Play);
        }
        
        public void EndGame()
        {
            photonView.RPC(nameof(ChangeState), RpcTarget.MasterClient, GameState.End);
        }
        
        [PunRPC]
        private void ChangeState(GameState state)
        {
            _sharedModel.SetState(state);
        }
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            ConnectedEvent?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            RoomJoinEvent?.Invoke(true, string.Empty);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnCreateRoomFailed");
            RoomJoinEvent?.Invoke(false, "message");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnCreateRoomFailed");
            RoomJoinEvent?.Invoke(false, message);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            RoomJoinEvent?.Invoke(false, message);
        }
    }
}