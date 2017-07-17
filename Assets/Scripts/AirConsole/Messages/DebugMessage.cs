using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class DebugMessage : Message
    {
        public override MessageType type => MessageType.Debug;

        public string message { get; set; } = string.Empty;

        public DebugMessage(JToken data)
        {
            message = (string)data["message"];
        }

        public DebugMessage()
        {
        }

        public override string ToString()
        {
            return $"DebugMessage({message})";
        }
    }
}
