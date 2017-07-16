using System;

using Newtonsoft.Json.Linq;

namespace CatFight
{
    public sealed class MessageEvent : EventArgs
    {
        public int From { get; }

        public JToken Data { get; }

        public MessageEvent(int from, JToken data)
        {
            From = from;
            Data = data;
        }
    }
}
