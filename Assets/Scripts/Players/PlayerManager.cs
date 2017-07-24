using System;
using System.Collections.Generic;
using System.Linq;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Players
{
    public sealed class PlayerManager : SingletonBehavior<PlayerManager>
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public IReadOnlyDictionary<int, Player> Players => _players;

        private readonly Dictionary<int, Player> _connectedPlayers = new Dictionary<int, Player>();

        private readonly Dictionary<int, Player> _disconnectedPlayers = new Dictionary<int, Player>();

        private readonly Dictionary<PlayerTeam.TeamIds, List<Player>> _teams = new Dictionary<PlayerTeam.TeamIds, List<Player>>();

        private Player _masterPlayer;

#region  Unity Lifecycle
        private void Awake()
        {
            foreach(PlayerTeam.TeamIds teamId in Enum.GetValues(typeof(PlayerTeam.TeamIds))) {
                if(PlayerTeam.TeamIds.None == teamId) {
                    continue;
                }
                _teams.Add(teamId, new List<Player>());
            }
        }
#endregion

        public void ConnectPlayer(int deviceId, out bool isReconnect)
        {
            isReconnect = false;

            if(ReconnectPlayer(deviceId)) {
                isReconnect = true;
                return;
            }

            Player player = new Player(deviceId, DataManager.Instance.GameData.schematic)
            {
                IsConnected = true
            };
            _players.Add(deviceId, player);

            _connectedPlayers.Add(deviceId, player);

            SetPlayerTeam(player);
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
                Debug.LogError($"Disconnecting unknown player {deviceId} (already disconnected: {_disconnectedPlayers.ContainsKey(deviceId)})!");
                return;
            }

            _disconnectedPlayers.Add(deviceId, player);
            _connectedPlayers.Remove(deviceId);

            if(player == _masterPlayer) {
                _masterPlayer = null;
                FindNewMasterPlayer();
            }
        }

        public void ConfirmPlayerSchematic(int deviceId, bool isConfirmed)
        {
            Player player;
            if(!_connectedPlayers.TryGetValue(deviceId, out player)) {
                Debug.LogError($"Cannot confirm non-connected player {deviceId} schematic!");
                return;
            }

            player.Schematic.IsConfirmed = isConfirmed;
        }

        public PlayerTeam.TeamIds GetPlayerTeam(int deviceId)
        {
            Player player;

            if (!_connectedPlayers.TryGetValue(deviceId, out player))
            {
                Debug.LogError($"Cannot confirm non-connected player {deviceId} Team ID retunring none!");
                return PlayerTeam.TeamIds.None;
            }

            return player.Team.Id;
        }

        public bool AreAllPlayersReady()
        {
            foreach(var kvp in _connectedPlayers) {
                if(!kvp.Value.Schematic.IsConfirmed) {
                    return false;
                }
            }
            return true;
        }

        private void SetPlayerTeam(Player player)
        {
            PlayerTeam.TeamIds smallestTeamId = PlayerTeam.TeamIds.TeamA;
            List<Player> smallestTeam = null;

            foreach(var kvp in _teams) {
                if(null == smallestTeam || kvp.Value.Count < smallestTeam.Count) {
                    smallestTeamId = kvp.Key;
                    smallestTeam = kvp.Value;
                }
            }

            if(null != smallestTeam) {
                player.Team.Id = smallestTeamId;
                smallestTeam.Add(player);
            }
        }

        public IReadOnlyCollection<Player> GetTeam(PlayerTeam.TeamIds teamId)
        {
            return _teams[teamId];
        }

        public void BroadcastToTeam(PlayerTeam.TeamIds teamId, Message message, int exceptDeviceId=-1)
        {
            List<Player> players;
            if(!_teams.TryGetValue(teamId, out players)) {
                Debug.LogError($"Unable to broadcast message to non-existant team {teamId}!");
                return;
            }

            foreach(Player player in players) {
                if(exceptDeviceId > 0 && player.DeviceId == exceptDeviceId) {
                    continue;
                }
                AirConsoleManager.Instance.Message(player.DeviceId, message);
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
    }
}
