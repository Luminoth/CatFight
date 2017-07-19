using System;

using JetBrains.Annotations;

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
                case Message.MessageType.StartGame:
                    return new StartGameMessage(data);
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
            None = 0,
            StartGame,
        }

        public abstract MessageType type { get; }

        [CanBeNull]
        public JToken Data { get; }

        protected Message(JToken data)
        {
            Data = data;
        }

        protected Message()
        {
        }
    }
}
