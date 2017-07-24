using System;

using CatFight.Data;
using CatFight.Players.Schematics;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    public static class LoadoutSlotFactory
    {
        public static LoadoutSlot Create(SchematicSlotData slotData)
        {
            switch(slotData.Type)
            {
            case SchematicSlotData.SchematicSlotTypeBrain:
                return new BrainLoadoutSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeWeapon:
                return new WeaponLoadoutSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeArmor:
                return new ArmorLoadoutSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeSpecial:
                return new SpecialLoadoutSlot(slotData);
            default:
                Debug.LogError($"Invalid schematic slot type {slotData.Type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class LoadoutSlot
    {
        [SerializeField]
        [ReadOnly]
        private SchematicSlotData _slotData;

        public SchematicSlotData SlotData { get { return _slotData; } private set { _slotData = value; } }

        public abstract void Process(SchematicSlot schematicSlot);

        public abstract void Complete();

        protected LoadoutSlot(SchematicSlotData slotData)
        {
            SlotData = slotData;
        }
    }
}
