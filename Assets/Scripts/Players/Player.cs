using System;

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
        private TeamData.TeamDataEntry _team;

        public TeamData.TeamDataEntry Team { get { return _team; } private set { _team = value; } }

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

        public void SetTeam(TeamData.TeamDataEntry team)
        {
            Team = team;
            AirConsoleManager.Instance.Message(DeviceId, new SetTeamMessage(team));
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
