using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class ConfirmStagingMessage : Message
    {
        public override MessageType type => MessageType.ConfirmStaging;

        public ConfirmStagingMessage(JToken data)
            : base(data)
        {
        }

        public ConfirmStagingMessage()
        {
        }

        public override string ToString()
        {
            return "ConfirmStagingMessage()";
        }
    }
}
