using System;
using System.Collections.Generic;
using System.Text;

namespace CatFight.Data
{
    [Serializable]
    public sealed class SchematicSlotData : Data
    {
        public const string SchematicSlotTypeWeapon = "weapon";
        public const string SchematicSlotTypeArmor = "armor";
        public const string SchematicSlotTypeCore = "core";

        public string type = SchematicSlotTypeWeapon;

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Slot({id}: {name} - {type})";
        }
    }

    [Serializable]
    public sealed class SchematicData : IData
    {
        public int maxFilledSlots = 1;

        public SchematicSlotData[] slots = new SchematicSlotData[0];

        private readonly Dictionary<int, SchematicSlotData> _slots = new Dictionary<int, SchematicSlotData>();

        public void Process()
        {
            foreach(SchematicSlotData slotData in slots) {
                slotData.Process();
                _slots.Add(slotData.id, slotData);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Max filled schematic slots: {maxFilledSlots}");

            builder.AppendLine("Schematic Slots:");
            foreach(SchematicSlotData slotData in slots) {
                builder.AppendLine(slotData.ToString());
            }

            return builder.ToString();
        }
    }
}
