using System.Collections.Generic;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Util;

using UnityEngine;

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

#region Unity Lifecycle
        private void Start()
        {
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
                return;
            }

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
            AirConsoleController.Instance.Message(evt.From, new DebugMessage
                {
                    message = $"Hello World {evt.From}"
                }
            );
        }
#endregion
    }
}
