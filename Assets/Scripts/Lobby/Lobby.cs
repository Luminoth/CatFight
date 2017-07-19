using System.Collections.Generic;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Lobby
{
    public sealed class Lobby : SingletonBehavior<Lobby>
    {
        [SerializeField]
        private LobbyPlayer _lobbyPlayerPrefab;

        public LobbyPlayer LobbyPlayerPrefab => _lobbyPlayerPrefab;

        [SerializeField]
        private LobbyPlayerList[] _playerLists;

        // deviceId => playerList
        private readonly Dictionary<int, LobbyPlayerList> _playerListMapping = new Dictionary<int, LobbyPlayerList>();

        [SerializeField]
        private Text _playerCountText;

#region Unity Lifecycle
        private void Start()
        {
            _playerCountText.text = "0";

            AirConsoleController.Instance.ConnectEvent += ConnectEventHandler;
            AirConsoleController.Instance.DisconnectEvent += DisconnectEventHandler;
            AirConsoleController.Instance.MessageEvent += MessageEventHandler;
        }

        protected override void OnDestroy()
        {
            if(AirConsoleController.HasInstance) {
                AirConsoleController.Instance.MessageEvent -= MessageEventHandler;
                AirConsoleController.Instance.DisconnectEvent -= DisconnectEventHandler;
                AirConsoleController.Instance.ConnectEvent -= ConnectEventHandler;
            }
        }
#endregion

        public bool IsFull()
        {
            bool isFull = false;
            foreach(LobbyPlayerList playerList in _playerLists) {
                isFull = isFull || playerList.IsFull;
            }
            return isFull;
        }

        private void AddPlayer(int deviceId)
        {
            foreach(LobbyPlayerList playerList in _playerLists) {
                if(playerList.IsFull) {
                    continue;
                }

                playerList.AddPlayer(deviceId);
                _playerListMapping.Add(deviceId, playerList);
                _playerCountText.text = _playerListMapping.Count.ToString();

                return;
            }

// TODO: lobby is full, need to send a message to the controller
// and remove them from the player manager
        }

        private void ReconnectPlayer(int deviceId)
        {
            LobbyPlayerList playerList;
            if(!_playerListMapping.TryGetValue(deviceId, out playerList)) {
                Debug.LogError($"Unable to reconnect non-existant player {deviceId}!");
                return;
            }
            playerList.ReconnectPlayer(deviceId);
        }

        private void DisconnectPlayer(int deviceId)
        {
            LobbyPlayerList playerList;
            if(!_playerListMapping.TryGetValue(deviceId, out playerList)) {
                Debug.LogError($"Unable to disconnect non-existant player {deviceId}!");
                return;
            }
            playerList.DisconnectPlayer(deviceId);
        }

        private void StartGame()
        {
            GameStageManager.Instance.LoadStaging();  
        }

#region Event Handlers
        private void ConnectEventHandler(object sender, ConnectEventArgs evt)
        {
            if(evt.IsReconnect) {
                ReconnectPlayer(evt.DeviceId);
            } else {
                AddPlayer(evt.DeviceId);
            }
        }

        private void DisconnectEventHandler(object sender, DisconnectEventArgs evt)
        {
            DisconnectPlayer(evt.DeviceId);
        }

        private void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
            case Message.MessageType.StartGame:
                StartGame();
                break;
            default:
                Debug.LogWarning($"Ignoring message {evt.Message} from {evt.From}");
                break;
            }
        }
#endregion
    }
}
