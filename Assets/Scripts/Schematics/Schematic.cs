using System.Collections.Generic;

using CatFight.Data;

using UnityEngine;

namespace CatFight.Schematics
{
    public sealed class Schematic
    {
        private readonly SchematicData _schematicData;

        private readonly Dictionary<int, SchematicSlot> _slots = new Dictionary<int, SchematicSlot>();

        private int _filledSlotCount;

        public Schematic(SchematicData data)
        {
            _schematicData = data;

            foreach(SchematicSlotData slotData in _schematicData.slots) {
                //Debug.Log($"Adding schematic slot {slotData.id}: {slotData.name} - {slotData.type}");
                _slots.Add(slotData.id, SchematicSlotFactory.Create(slotData));
            }
        }

        public bool SetSlot(int slotId, int itemId)
        {
            if(_filledSlotCount >= _schematicData.maxFilledSlots) {
                return false;
            }

// TODO: error check
            _slots[slotId].ItemId = itemId;
            ++_filledSlotCount;

            return true;

        }

        public void ClearSlot(int slotId)
        {
// TODO: error check
            _slots[slotId].ItemId = 0;
            --_filledSlotCount;
        }
    }
}
