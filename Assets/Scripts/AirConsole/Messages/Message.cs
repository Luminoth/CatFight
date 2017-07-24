using System;

using JetBrains.Annotations;

using Newtonsoft.Json;
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
                case Message.MessageType.ConfirmStaging:
                    return new ConfirmStagingMessage(data);
                case Message.MessageType.SetTeam:
                    return new SetTeamMessage(data);
                case Message.MessageType.SetSlot:
                    return new SetSlotMessage(data);
                case Message.MessageType.ClearSlot:
                    return new ClearSlotMessage(data);
                case Message.MessageType.ControllerAction:
                    return new SetInputMessage(data);
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
            ConfirmStaging,
            SetTeam,
            SetSlot,
            ClearSlot,
            ControllerAction
        }

        public abstract MessageType type { get; }

        [CanBeNull]
        [JsonIgnore]
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
