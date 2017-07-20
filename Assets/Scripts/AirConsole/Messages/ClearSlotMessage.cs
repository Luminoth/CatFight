using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class ClearSlotMessage : Message
    {
        public override MessageType type => MessageType.ClearSlot;

        public int slotId { get; set; }

        public ClearSlotMessage(JToken data)
            : base(data)
        {
            slotId = (int)data["slotId"];
        }

        public ClearSlotMessage()
        {
        }

        public override string ToString()
        {
            return $"ClearSlotMessage({slotId})";
        }
    }
}
