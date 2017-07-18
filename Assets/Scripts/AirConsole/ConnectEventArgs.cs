using System;

namespace CatFight.AirConsole
{
    public sealed class ConnectEventArgs : EventArgs
    {
        public int DeviceId { get; }

        public bool IsReconnect { get; }

        public ConnectEventArgs(int deviceId, bool isReconnect)
        {
            DeviceId = deviceId;
            IsReconnect = isReconnect;
        }
    }
}
