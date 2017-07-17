using System;

using Newtonsoft.Json.Linq;

using UnityEngine;

namespace CatFight.AirConsole.Messages
{
    public static class MessageFactory
    {
        public static Message Parse(JToken data)
        {
            try {
                int type = (int)data["type"];
                Message.MessageType messageType = (Message.MessageType)type;

                switch(messageType)
                {
                case Message.MessageType.Debug:
                    return new DebugMessage(data);
                default:
                    Debug.LogError($"Unsupported message type: {messageType}");
                    return null;
                }
            } catch(Exception e) {
                Debug.LogError($"Caught exception while parsing message: {e}");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class Message
    {
        public enum MessageType
        {
            None,
            Debug,
        }

        public abstract MessageType type { get; }
    }
}
