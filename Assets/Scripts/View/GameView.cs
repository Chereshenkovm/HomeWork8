using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace View
{
    public class GameView : MonoBehaviour
    {
        public event Action CreateGameClickEvent;
        public event Action FindRandomGameEvent;
        public event Action StartGameEvent;
        
        [SerializeField] private ZombieMap _zombieMap;
        
        [SerializeField] private GameObject _winBlock;
        [SerializeField] private GameObject _gameOverBlock;
        
        
        [SerializeField] private GameObject _startBlock;
        [SerializeField] private GameObject _startButtonBlock;
        [SerializeField] private GameObject _waitPlayersBlock;
        
        [SerializeField] private GameObject _loadingBlock;
        [SerializeField] private GameObject _settingsBlock;
        
        
        [SerializeField] private InputField _playerName;
        [SerializeField] private InputField _roomName;
        [SerializeField] private Text _error;

        public string PlayerName => _playerName.text;
        public string RoomName => _roomName.text;

        
        public void SetLoadingState(bool active)
        {
            _loadingBlock.SetActive(active);
        }

        public void SetSettingsState(bool active)
        {
            _settingsBlock.SetActive(active);
        }

        public void SetError(string error)
        {
            _error.text = error;
        }

        public void SetWinState(bool show, bool win)
        {
            _winBlock.SetActive(show && win);
            _gameOverBlock.SetActive(show && !win);
        }

        public void SetStartState(bool show, bool startButton)
        {
            _startBlock.SetActive(show);
            _startButtonBlock.SetActive(startButton);
            _waitPlayersBlock.SetActive(!startButton);
        }
        
        public void OnCreateGameClick()
        {
            CreateGameClickEvent?.Invoke();
        }
	
        public void OnFindRandomGameClick()
        {
            FindRandomGameEvent?.Invoke();
        }

        public void OnStartGameClick()
        {
            StartGameEvent?.Invoke();
        }
    }
}