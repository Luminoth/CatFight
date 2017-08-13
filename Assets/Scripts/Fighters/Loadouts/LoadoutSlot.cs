using System;

using CatFight.Data;
using CatFight.Players.Schematics;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    public static class LoadoutSlotFactory
    {
        public static LoadoutSlot Create(Fighter fighter, SchematicSlotData slotData)
        {
            switch(slotData.Type)
            {
            case SchematicSlotData.SlotType.Brain:
                return new BrainLoadoutSlot(fighter, slotData);
            case SchematicSlotData.SlotType.Weapon:
                return new WeaponLoadoutSlot(fighter, slotData);
            case SchematicSlotData.SlotType.Armor:
                return new ArmorLoadoutSlot(fighter, slotData);
            case SchematicSlotData.SlotType.Special:
                return new SpecialLoadoutSlot(fighter, slotData);
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

        public Fighter Fighter { get; }

        public abstract void Process(SchematicSlot schematicSlot);

        public abstract void Complete();

        protected LoadoutSlot(Fighter fighter, SchematicSlotData slotData)
        {
            Fighter = fighter;
            SlotData = slotData;
        }
    }
}
