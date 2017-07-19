using CatFight.AirConsole;
using CatFight.Data;
using CatFight.Schematics;

namespace CatFight
{
    public sealed class Player
    {
        public int DeviceId { get; }

        public bool IsConnected { get; set; }

        public bool IsMasterPlayer { get; private set; }

        public Schematic Schematic { get; }

        public Player(int deviceId, SchematicData schematicData)
        {
            DeviceId = deviceId;
            Schematic = new Schematic(schematicData);
        }

        public void SetMasterPlayer(bool isMasterPlayer)
        {
            IsMasterPlayer = isMasterPlayer;

            if(IsMasterPlayer) {
                AirConsoleController.Instance.SetMasterPlayer(DeviceId);
            }
        }
    }
}
