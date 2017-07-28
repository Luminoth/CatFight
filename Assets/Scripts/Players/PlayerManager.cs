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

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private Player[] _debugPlayers;
#endif

        private readonly Dictionary<int, Player> _connectedPlayers = new Dictionary<int, Player>();

        private readonly Dictionary<int, Player> _disconnectedPlayers = new Dictionary<int, Player>();

        private readonly Dictionary<Player.TeamIds, List<Player>> _teams = new Dictionary<Player.TeamIds, List<Player>>();

        private Player _masterPlayer;

#region  Unity Lifecycle
        private void Awake()
        {
            foreach(Player.TeamIds teamId in Enum.GetValues(typeof(Player.TeamIds))) {
                if(Player.TeamIds.None == teamId) {
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

            Player player = new Player(deviceId, DataManager.Instance.GameData.Fighter.Schematic)
            {
                IsConnected = true
            };
            _players.Add(deviceId, player);

#if UNITY_EDITOR
            _debugPlayers = _players.Values.ToArray();
#endif

            _connectedPlayers.Add(deviceId, player);

            SetPlayerTeam(player);
            SetMasterPlayer(player);
        }

        private bool ReconnectPlayer(int deviceId)
        {
            Player player = _disconnectedPlayers.GetOrDefault(deviceId);
            if(null == player) {
                return false;
            }

            Debug.Log($"OnReconnect({deviceId})");

            _disconnectedPlayers.Remove(deviceId);
            _connectedPlayers.Add(deviceId, player);

            player.IsConnected = true;

            // update the player's master status
            if(player.IsMasterPlayer && null != _masterPlayer && player != _masterPlayer) {
                player.SetMasterPlayer(false);
            }

            SetMasterPlayer(player);

            return true;
        }

        public void DisconnectPlayer(int deviceId)
        {
            Player player = _connectedPlayers.GetOrDefault(deviceId);
            if(null == player) {
                Debug.LogError($"Disconnecting unknown player {deviceId} (already disconnected: {_disconnectedPlayers.ContainsKey(deviceId)})!");
                return;
            }

            _disconnectedPlayers.Add(deviceId, player);
            _connectedPlayers.Remove(deviceId);

            player.IsConnected = false;

            if(player == _masterPlayer) {
                _masterPlayer = null;
                FindNewMasterPlayer();
            }
        }

        public void ResetPlayers()
        {
            foreach(var kvp in _players) {
                kvp.Value.Reset();
            }
        }

        public void ConfirmPlayerSchematic(int deviceId, bool isConfirmed)
        {
            Player player = _connectedPlayers.GetOrDefault(deviceId);
            if(null == player) {
                Debug.LogError($"Cannot confirm non-connected player {deviceId} schematic!");
                return;
            }

            player.Schematic.IsConfirmed = isConfirmed;
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
            Player.TeamIds smallestTeamId = Player.TeamIds.TeamA;
            List<Player> smallestTeam = null;

            foreach(var kvp in _teams) {
                if(null == smallestTeam || kvp.Value.Count < smallestTeam.Count) {
                    smallestTeamId = kvp.Key;
                    smallestTeam = kvp.Value;
                }
            }

            if(null != smallestTeam) {
                player.SetTeam(smallestTeamId);
                smallestTeam.Add(player);
            }
        }

        public IReadOnlyCollection<Player> GetTeam(Player.TeamIds teamId)
        {
            return _teams[teamId];
        }

        public void BroadcastToTeam(Player.TeamIds teamId, Message message, int exceptDeviceId=-1)
        {
            var players = _teams.GetOrDefault(teamId);
            if(null == players) {
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
