using System.Collections.Generic;
using System.Linq;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Players
{
    public sealed class PlayerManager : SingletonBehavior<PlayerManager>
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        private readonly List<Player> _playerList = new List<Player>();

        public IReadOnlyCollection<Player> Players => _playerList;

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private Player[] _debugPlayers;
#endif

#region Connected Players
        private readonly Dictionary<int, Player> _connectedPlayers = new Dictionary<int, Player>();

        private readonly List<Player> _connectedPlayersList = new List<Player>();
#endregion

        private readonly Dictionary<int, Player> _disconnectedPlayers = new Dictionary<int, Player>();

        private Player _masterPlayer;

#region Teams
        private readonly Dictionary<int, Team> _teams = new Dictionary<int, Team>();

        private readonly List<Team> _teamsList = new List<Team>();

        public IReadOnlyCollection<Team> Teams => _teamsList;
#endregion

#region  Unity Lifecycle
        private void Awake()
        {
            foreach(TeamData.TeamDataEntry teamData in DataManager.Instance.GameData.Teams.Teams) {
                Team team = new Team(teamData.Id);

                _teams.Add(teamData.Id, team);
                _teamsList.Add(team);
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
            _playerList.Add(player);

#if UNITY_EDITOR
            _debugPlayers = Players.ToArray();
#endif

            _connectedPlayers.Add(deviceId, player);
            _connectedPlayersList.Add(player);

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
            _connectedPlayersList.Add(player);

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
            _connectedPlayersList.Remove(player);

            player.IsConnected = false;

            if(player == _masterPlayer) {
                _masterPlayer = null;
                FindNewMasterPlayer();
            }
        }

        [CanBeNull]
        public Player GetPlayer(int deviceId)
        {
            return _players.GetOrDefault(deviceId);
        }

        public void ResetPlayers()
        {
            foreach(Player player in Players) {
                player.Reset();
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
// TODO: this is better handled with an event
            foreach(Player player in _connectedPlayersList) {
                if(!player.Schematic.IsConfirmed) {
                    return false;
                }
            }
            return true;
        }

        private void SetPlayerTeam(Player player)
        {
            int smallestTeamId = 0;
            Team smallestTeam = null;

            foreach(Team team in _teamsList) {
                if(null == smallestTeam || team.Players.Count < smallestTeam.Players.Count) {
                    smallestTeamId = team.TeamId;
                    smallestTeam = team;
                }
            }

            if(null != smallestTeam) {
                player.SetTeam(DataManager.Instance.GameData.Teams.Entries[smallestTeamId]);
                smallestTeam.AddPlayer(player);
            }
        }

        public Team GetTeam(int teamId)
        {
            return _teams.GetOrDefault(teamId);
        }

        public void BroadcastToTeam(int teamId, Message message, int exceptDeviceId=-1)
        {
            Team team = _teams.GetOrDefault(teamId);
            if(null == team) {
                Debug.LogError($"Unable to broadcast message to non-existant team {teamId}!");
                return;
            }

            foreach(Player player in team.Players) {
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
            if(_connectedPlayersList.Count < 1) {
                return;
            }
            SetMasterPlayer(_connectedPlayersList.First());
        }
    }
}
