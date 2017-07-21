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
            case SchematicSlotData.SchematicSlotTypeWeapon:
                return new WeaponLoadoutSlot();
            case SchematicSlotData.SchematicSlotTypeArmor:
                return new ArmorLoadoutSlot();
            case SchematicSlotData.SchematicSlotTypeCore:
                return new CoreLoadoutSlot();
            default:
                Debug.LogError($"Invalid schematic slot type {slotData.Type}!");
                return null;
            }
        }
    }

    public abstract class LoadoutSlot
    {
        public abstract void Process(SchematicSlot schematicSlot);

        public abstract void Complete();
    }
}
