using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class SchematicSlotData : Data
    {
        public const string SchematicSlotTypeWeapon = "weapon";
        public const string SchematicSlotTypeArmor = "armor";
        public const string SchematicSlotTypeCore = "core";

        [SerializeField]
        private string _type = SchematicSlotTypeWeapon;

        public string Type => _type;

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Slot({Id}: {Name} - {Type})";
        }
    }

    [Serializable]
    public sealed class SchematicData : IData
    {
        [SerializeField]
        private int maxFilledSlots = 1;

        public int MaxFilledSlots => maxFilledSlots;

        [SerializeField]
        private SchematicSlotData[] slots = new SchematicSlotData[0];

        public IReadOnlyCollection<SchematicSlotData> Slots => slots;

        private readonly Dictionary<int, SchematicSlotData> _slotData = new Dictionary<int, SchematicSlotData>();

        public void Process()
        {
            foreach(SchematicSlotData slotData in slots) {
                slotData.Process();
                _slotData.Add(slotData.Id, slotData);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Max filled schematic slots: {MaxFilledSlots}");

            builder.AppendLine("Schematic Slots:");
            foreach(SchematicSlotData slotData in slots) {
                builder.AppendLine(slotData.ToString());
            }

            return builder.ToString();
        }
    }
}
