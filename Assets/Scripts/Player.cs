namespace CatFight
{
    public sealed class Player
    {
        public int DeviceId { get; }

        public bool IsConnected { get; set; }

        public bool IsMasterPlayer { get; private set; }

        public Schematic Schematic => new Schematic();

        public Player(int deviceId)
        {
            DeviceId = deviceId;
        }

        public void SetMasterPlayer(bool isMasterPlayer)
        {
            IsMasterPlayer = isMasterPlayer;

            // TODO: notify the device
        }
    }
}
