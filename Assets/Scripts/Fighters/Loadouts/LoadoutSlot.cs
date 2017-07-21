using CatFight.Data;
using CatFight.Players.Schematics;

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

    public abstract class LoadoutSlot
    {
        public SchematicSlotData SlotData { get; }

        public abstract void Process(SchematicSlot schematicSlot);

        public abstract void Complete();

        protected LoadoutSlot(SchematicSlotData slotData)
        {
            SlotData = slotData;
        }
    }
}
