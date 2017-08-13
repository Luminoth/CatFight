using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using CatFight.Fighters.Loadouts;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class SchematicSlotData
    {
        public enum SlotType
        {
            [Description("none")]
            None,

            [Description("brain")]
            Brain,

            [Description("armor")]
            Weapon,

            [Description("armor")]
            Armor,

            [Description("special")]
            Special
        }

        [SerializeField]
        private int _id;

        public int Id => _id;

        [SerializeField]
        private string _name;

        public string Name => _name;

        [SerializeField]
        private SlotType _type = SlotType.None;

        [JsonConverter(typeof(StringEnumConverter))]
        public SlotType Type => _type;

        [SerializeField]
        private LoadoutSlotItem[] _slotItemPrefabs;

        [JsonIgnore]
        public LoadoutSlotItem[] SlotItemPrefabs => _slotItemPrefabs;

        public override string ToString()
        {
            return $"Slot({Id}: {Name} - {Type})";
        }
    }

    [Serializable]
    public sealed class SchematicData
    {
        [SerializeField]
        private int _maxFilledSlots = 1;

        public int MaxFilledSlots => _maxFilledSlots;

        [SerializeField]
        private SchematicSlotData[] _slots;

        public IReadOnlyCollection<SchematicSlotData> Slots => _slots;

        private readonly Dictionary<int, SchematicSlotData> _entries = new Dictionary<int, SchematicSlotData>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, SchematicSlotData> Entries => _entries;

        private void Awake()
        {
            foreach(SchematicSlotData entry in Slots) {
                _entries.Add(entry.Id, entry);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Max filled schematic slots: {MaxFilledSlots}");

            builder.AppendLine("Schematic Slots:");
            foreach(SchematicSlotData slotData in Slots) {
                builder.AppendLine(slotData.ToString());
            }

            return builder.ToString();
        }
    }
}
