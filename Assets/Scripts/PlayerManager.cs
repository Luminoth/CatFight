using System.Collections.Generic;
using System.Linq;

using CatFight.AirConsole;

using UnityEngine;

namespace CatFight
{
    public sealed class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        private readonly Dictionary<int, Player> _connectedPlayers = new Dictionary<int, Player>();
        private readonly Dictionary<int, Player> _disconnectedPlayers = new Dictionary<int, Player>();

        private Player _masterPlayer;

        public void ConnectPlayer(int deviceId, out bool isReconnect)
        {
            isReconnect = false;

            if(ReconnectPlayer(deviceId)) {
                isReconnect = true;
                return;
            }

            Player player = new Player(deviceId, AirConsoleController.Instance.GameData.schematic)
            {
                IsConnected = true
            };
            _connectedPlayers.Add(deviceId, player);

            SetMasterPlayer(player);
        }

        private bool ReconnectPlayer(int deviceId)
        {
            Player player;
            if(!_disconnectedPlayers.TryGetValue(deviceId, out player)) {
                return false;
            }

            Debug.Log($"OnReconnect({deviceId})");

            _disconnectedPlayers.Remove(deviceId);
            _connectedPlayers.Add(deviceId, player);

            // update the player's master status
            if(player.IsMasterPlayer && null != _masterPlayer && player != _masterPlayer) {
                player.SetMasterPlayer(false);
            }

            SetMasterPlayer(player);

            return true;
        }

        public void DisconnectPlayer(int deviceId)
        {
            Player player;
            if(!_connectedPlayers.TryGetValue(deviceId, out player)) {
                Debug.LogError($"Disconnecting unknown player {deviceId}!");
                return;
            }

            _disconnectedPlayers.Add(deviceId, player);
            _connectedPlayers.Remove(deviceId);

            if(player == _masterPlayer) {
                _masterPlayer = null;
                FindNewMasterPlayer();
            }
        }

        private void SetMasterPlayer(Player player, bool force=false)
        {
            if(null != _masterPlayer && !force) {
                return;
            }

            _masterPlayer?.SetMasterPlayer(false);

            _masterPlayer = player;
            player.SetMasterPlayer(true);
        }

        private void FindNewMasterPlayer()
        {
            if(_connectedPlayers.Count < 1) {
                return;
            }
            SetMasterPlayer(_connectedPlayers.First().Value);
        }

        private PlayerManager()
        {
        }
    }
}
