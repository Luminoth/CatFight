using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
// TODO: this should be a custom state value for the controller
// (and that property should reset to false when the staging view comes up)
    [Serializable]
    public sealed class ConfirmStagingMessage : Message
    {
        public override MessageType type => MessageType.ConfirmStaging;

        public bool isConfirmed { get; set; }

        public ConfirmStagingMessage(JToken data)
            : base(data)
        {
            isConfirmed = (bool)data["isConfirmed"];
        }

        public ConfirmStagingMessage()
        {
        }

        public override string ToString()
        {
            return $"ConfirmStagingMessage({isConfirmed})";
        }
    }
}
