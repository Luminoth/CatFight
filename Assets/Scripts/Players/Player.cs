using System;
using System.ComponentModel;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Players.Schematics;

using UnityEngine;

namespace CatFight.Players
{
    [Serializable]
    public sealed class Player
    {
        public enum TeamIds
        {
            [Description("None")]
            None = 0,

            [Description("Team A")]
            TeamA = 1,

            [Description("Team B")]
            TeamB = 2
        }

        [SerializeField]
        [Util.ReadOnly]
        private int _deviceId;

        public int DeviceId { get { return _deviceId; } private set { _deviceId = value; } }

        [SerializeField]
        [Util.ReadOnly]
        private bool _isConnected;

        public bool IsConnected { get { return _isConnected; } set { _isConnected = value; } }

        [SerializeField]
        [Util.ReadOnly]
        private bool _isMasterPlayer;

        public bool IsMasterPlayer { get { return _isMasterPlayer; } private set { _isMasterPlayer = value; } }

        [SerializeField]
        [Util.ReadOnly]
        private TeamIds _teamId = TeamIds.TeamA;

        public TeamIds TeamId { get { return _teamId; } private set { _teamId = value; } }

        [SerializeField]
        [Util.ReadOnly]
        private Schematic _schematic;

        public Schematic Schematic { get { return _schematic; } private set { _schematic = value; } }

        public Player(int deviceId, SchematicData schematicData)
        {
            DeviceId = deviceId;
            Schematic = new Schematic(this, schematicData);
        }

        public void Reset()
        {
            Schematic.Reset();
        }

        public void SetTeam(TeamIds teamId)
        {
            TeamId = teamId;
            AirConsoleManager.Instance.Message(DeviceId, new SetTeamMessage(TeamId));
        }

        public void SetMasterPlayer(bool isMasterPlayer)
        {
            IsMasterPlayer = isMasterPlayer;

            if(IsMasterPlayer) {
                AirConsoleManager.Instance.SetMasterPlayer(DeviceId);
            }
        }
    }
}
