using System;

namespace CatFight.AirConsole
{
    public sealed class DisconnectEventArgs : EventArgs
    {
        public int DeviceId { get; }

        public DisconnectEventArgs(int deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
