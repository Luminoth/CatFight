using System;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class SetSlotMessage : Message
    {
        public override MessageType type => MessageType.SetSlot;

        public int slotId { get; set; }

        public int itemId { get; set; }

        public SetSlotMessage(JToken data)
            : base(data)
        {
            slotId = (int)data["slotId"];
            itemId = (int)data["itemId"];
        }

        public SetSlotMessage()
        {
        }

        public override string ToString()
        {
            return $"SetSlotMessage({slotId}: {itemId})";
        }
    }
}
