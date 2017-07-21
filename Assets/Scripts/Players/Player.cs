using CatFight.AirConsole;
using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Players
{
    public sealed class Player
    {
        public int DeviceId { get; }

        public bool IsConnected { get; set; }

        public bool IsMasterPlayer { get; private set; }

        public PlayerTeam Team { get; }

        public Schematic Schematic { get; }

        public Player(int deviceId, SchematicData schematicData)
        {
            DeviceId = deviceId;
            Team = new PlayerTeam(this);
            Schematic = new Schematic(this, schematicData);
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
