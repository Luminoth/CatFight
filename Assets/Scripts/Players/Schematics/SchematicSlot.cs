using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Players.Schematics
{
    public static class SchematicSlotFactory
    {
        [CanBeNull]
        public static SchematicSlot Create(SchematicSlotData slotData)
        {
            switch(slotData.Type)
            {
            case SchematicSlotData.SlotType.Brain:
                return new BrainSchematicSlot(slotData);
            case SchematicSlotData.SlotType.Weapon:
                return new WeaponSchematicSlot(slotData);
            case SchematicSlotData.SlotType.Armor:
                return new ArmorSchematicSlot(slotData);
            case SchematicSlotData.SlotType.Special:
                return new SpecialSchematicSlot(slotData);
            default:
                Debug.LogError($"Invalid schematic slot type {slotData.Type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class SchematicSlot
    {
        [SerializeField]
        [ReadOnly]
        private SchematicSlotData _slotData;

        public SchematicSlotData SlotData { get { return _slotData; } private set { _slotData = value; } }

        [SerializeField]
        [ReadOnly]
        private int _itemId;

        public int ItemId { get { return _itemId; } set { _itemId = value; } }

        public bool IsFilled => ItemId > 0;

        public void Clear()
        {
            ItemId = 0;
        }

        protected SchematicSlot(SchematicSlotData slotData)
        {
            SlotData = slotData;
        }
    }
}
