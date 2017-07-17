using System;

using CatFight.AirConsole.Messages;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole
{
    public sealed class MessageEvent : EventArgs
    {
        public int From { get; }

        public Message Message { get; }

        public MessageEvent(int from, JToken data)
        {
            From = from;
            Message = MessageFactory.Parse(data);
        }
    }
}
