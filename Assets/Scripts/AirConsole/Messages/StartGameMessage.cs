using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class StartGameMessage : Message
    {
        public override MessageType type => MessageType.StartGame;

        public StartGameMessage(JToken data)
            : base(data)
        {
        }

        public StartGameMessage()
        {
        }

        public override string ToString()
        {
            return "StartGameMessage()";
        }
    }
}
