using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole
{
    public sealed class CustomDeviceStateChangeEventArgs : EventArgs
    {
        public int DeviceId { get; }

        public CustomDeviceStateChangeEventArgs(int deviceId, JToken data)
        {
            DeviceId = deviceId;
        }
    }
}
